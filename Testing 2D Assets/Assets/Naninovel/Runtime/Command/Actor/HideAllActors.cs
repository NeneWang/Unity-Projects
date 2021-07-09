// Copyright 2017-2021 Elringus (Artyom Sovetnikov). All rights reserved.

using System.Linq;
using UniRx.Async;

namespace Naninovel.Commands
{
    /// <summary>
    /// Hides (removes) all the actors (characters, backgrounds, text printers, choice handlers) on scene.
    /// </summary>
    [CommandAlias("hideAll")]
    public class HideAllActors : Command
    {
        /// <summary>
        /// Duration (in seconds) of the fade animation. Default value: 0.35 seconds.
        /// </summary>
        [ParameterAlias("time"), ParameterDefaultValue("0.35")]
        public DecimalParameter Duration = .35f;
        /// <summary>
        /// Whether to remove (destroy) the actors after they are hidden.
        /// Use to unload resources associated with the actors and prevent memory leaks.
        /// </summary>
        [ParameterDefaultValue("false")]
        public BooleanParameter Remove = false;

        public override async UniTask ExecuteAsync (CancellationToken cancellationToken = default)
        {
            var managers = Engine.FindAllServices<IActorManager>();
            await UniTask.WhenAll(managers.SelectMany(m => m.GetAllActors()).Select(a => a.ChangeVisibilityAsync(false, Duration, cancellationToken: cancellationToken)));
            if (cancellationToken.CancelASAP) return;
            if (Remove)
                foreach (var manager in managers)
                    manager.RemoveAllActors();
        }
    }
}
