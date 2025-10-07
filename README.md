# Royal Core — Feature-Driven Game Framework for Unity

Royal Core is a **modular**, **loosely coupled** Unity framework built around a **Context-backed DI/Service Locator**, a clean **MVC** layer, and **Command/Port** based communication. It lets you assemble games as trees of **Features** (Core → Game → Lobby/Gameplay → Player/Enemy/…); each Feature has a deterministic lifecycle, resolves only the interfaces it needs, and can be added/removed on demand without touching unrelated code.

---

## ✨ Highlights

- **Feature Architecture**: Compose gameplay as independent Features with a clear lifecycle  
  `PreInstall → Install → Resolve → WarmupAsync → Start → Stop → Dispose`.
- **Context DI / Service Locator**: Local-first resolution with parent fallback; bind/import **Services, Controllers, Models, Ports** per Feature.
- **Ports & Commands**: Inter-feature communication via small interfaces and Commands (navigation, combat, UI), not hard references.
- **MVC/MVCS**: Thin Controllers, pooled Views, SO/POCO Models; interface segregation keeps modules reusable.

---

## Core Concepts

### Feature & Context

- A **Feature** is a modular unit (e.g., `PlayerFeature`, `EnemyFeature`).
- Each Feature owns a **Context** (local Service/Port/Model/Controller registries) with **local-first** resolve, then parent fallback.
- **Ports** are minimal public interfaces (e.g., `IHittable`, `IMovable`, `IGameNavPort`) that Features **Export** and others **Import**.

```csharp
// Exporting a port
Ctx.Export<IHittable>(healthController);

// Importing
var hit = Ctx.ImportPort<IHittable>();
```

#### Lifecycle (deterministic, async-friendly)

- **PreInstall:** optional declarations, non-IO planning
- **Install:** create & bind local objects (no IO)
- **Resolve:** wire external dependencies (Ports/Services)
- **WarmupAsync:** heavy work (Addressables, Scene load, pool prewarm)
- **Start/Stop:** subscriptions, ticking, gameplay on/off
- **Dispose:** tear down in reverse order (children first, then self)

---

## MVC in Practice

- **Model:** SO/POCO data (PlayerModel, MovementData).
- **View:** MonoBehaviours implementing small view interfaces (AgentView : IView, IAnimationView, IMovementView).
- **Controller:** Pure C# logic; receives only the interfaces it needs (IMovable, IHittable, IAnimationView), pulled from Context.

Controllers remain thin, specialized modules (movement, animation, health). A “main” controller can compose them and expose Ports outward (e.g., IPlayerPort : IMovable, IHittable).

---

## Communication: Ports & Commands

- **Ports:** Stable, minimal interface contracts (e.g., IHittablePort.Hit(args)).
- **Commands:** Intent objects processed by a handler in the proper Feature; good for UI → gameplay requests or cross-cutting actions.

This keeps dependencies directional and small, avoids tight coupling to concrete classes, and simplifies testing.

## Quick Start
1. Bootstrap Core
```csharp
public sealed class CoreBootstrapper : MonoBehaviour
    {
        private static bool _initialized;
        private IFeature _core;

        private void Awake()
        {
            if (_initialized)
            {
                Destroy(gameObject);
                return;
            }

            _initialized = true;
            DontDestroyOnLoad(gameObject);

            _core = new CoreFeature("Core", CancellationTokenSource.CreateLinkedTokenSource(Application.exitCancellationToken));
            _core.PreInstall();
            _core.Install();
            _core.Resolve();            
        }

        private void Start()
        {
            _core.Start();
        }

        private void OnDestroy()
        {
            _core?.Dispose();
            _core = null;
            _initialized = false;
        }
    }
```

```csharp
      public sealed class CoreFeature : BaseFeature
      {
        private CoreView _view;
        private CancellationTokenSource _ct;

        public CoreFeature(string address, CancellationTokenSource ct) : base(address)
        {
            _ct = ct;
        }

        protected override void OnPreInstall()
        {
            // PlanChild("GamePlay", (addr, p) => new GamePlayFeature(addr, p));
        }

        protected override void OnInstall()
        {
            var vp = new ResourcesViewProvider();
            Context.Services.Bind<IViewProvider>(vp);

            var mp = new ResourcesModelProvider();
            Context.Services.Bind<IModelProvider>(mp);

            Context.Services.Bind<ITimeService>(new UnityTimeService());

            var updateService = new UpdateService<IUpdatable>();
            Context.Services.Bind<IUpdateService<IUpdatable>>(updateService);
            Context.Services.Bind<IUpdateService<IFixedUpdatable>>(new UpdateService<IFixedUpdatable>());
            Context.Services.Bind<IUpdateService<ILateUpdatable>>(new UpdateService<ILateUpdatable>());

            Context.Services.Bind<ISceneLoaderService>(new SceneLoaderService());

            _view = vp.LoadView<CoreView>("Core/CoreView");
            Context.Views.Bind(_view);

            var canvasView = vp.LoadView<UIRoot>("Core/UIRoot");
            Context.Services.Bind<IUIService>(new UIService(vp, canvasView));

            var audioView = vp.LoadView<AudioView>("Core/AudioView");
            var audioService = new AudioService(audioView);
            Context.Services.Bind<IAudioService>(audioService);

            audioService.AddAudioClips(mp.LoadSO<AudioClipsScriptableObject>("Core/CoreAudioClips"));

        }

        protected override void OnStart()
        {
            Context.ImportService<ISceneLoaderService>().InitEntryPoint();

            _ = InitEntryPoint(_ct);
        }
        
        private async Awaitable InitEntryPoint(CancellationTokenSource ct)
        {
            Context.ImportService<IUIService>().PushPopup<LoadingView>("Core/LoadingView");

            var game = await Context.ImportService<IFeatureFactory>().CreateAsync(this, "Game",
                (addr, parent) => new GameFeature(addr, parent), ct);
            game.Start();
            
            Context.ImportService<IUIService>().PopPopup();
        }
    }
```

2. Example Feature: Input
```csharp
    public class InputFeature : BaseFeature
    {
        private readonly string _modelKey;
        private readonly string _viewKey;
        private InputController _ctrl;

        private InputModel _model;
        private JoystickView _view;

        public InputFeature(string address, IFeature parent,
            string viewKey = "JoystickView",
            string modelKey = "Configs/Input/InputModel")
            : base(address, parent)
        {
            _viewKey = viewKey;
            _modelKey = modelKey;
        }

        protected override void OnInstall()
        {
            var mp = Context.ImportService<IModelProvider>();
            var ui = Context.ImportService<IUIService>();

            _view = ui.Show<JoystickView>(_viewKey);
            Context.Views.Bind(_view);

            var so = mp.LoadSO<InputCongfigSO>(_modelKey);
            _model = new InputModel(so);
            Context.Models.Bind(_model);

            _ctrl = new InputController(_view, _model, Context.CommandFactory);
            Context.Controllers.Bind(_ctrl);

            Context.Ports.Bind<IJoystickInputPort>(_ctrl);
            Context.Export(Context.Ports.Require<IJoystickInputPort>());
        }

        protected override void OnStart()
        {
            Context.ImportService<IUpdateService<IUpdatable>>().RegisterUpdatable(_ctrl);
        }

        protected override void OnDispose()
        {
            Context.ImportService<IUpdateService<IUpdatable>>().UnregisterUpdatable(_ctrl);
            
            if (Context.TryImportService<IUIService>(out var uiService) && _view) uiService.Close(_view);
            _view = null;
        }
    }
```

---

## 🛠 Installation & Updates
TODO

##  📎 Why RoyalCore?
-	Speed: new games boot in minutes on a proven skeleton.
-	Safety: Ports/Commands keep dependencies minimal and explicit.
-	Scale: Async scene loading, pooling, and Feature lifecycle handle complex content smoothly.
-	Reuse: Movement/Health/Weapons/UI drop into any Feature with minimal glue.