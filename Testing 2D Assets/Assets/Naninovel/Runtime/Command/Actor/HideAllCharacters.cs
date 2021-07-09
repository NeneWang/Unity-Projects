// Copyright 2017-2021 Elringus (Artyom Sovetnikov). All rights reserved.

using System.Linq;
using UniRx.Async;

namespace Naninovel.Commands
{
    /// <summary>
    /// Hides (removes) all the visible characters on scene.
    /// </summary>
    [CommandAlias("hideChars")]
    public class HideAllCharacters : Command
    {
        /// <summary>
        /// Duration (in seconds) of the fade animation. Default value: 0.35 seconds.
        /// </summary>
        [ParameterAlias("time"), ParameterDefaultValue("0.35")]
        public DecimalParameter Duration = .35f;
        /// <summary>
        /// Whether to remove (destroy) the characters after they are hidden.
        /// Use to unload resources associated with the characters and prevent memory leaks.
        /// </summary>
        [ParameterDefaultValue("false")]
        public BooleanParameter Remove = false;

        public override async UniTask ExecuteAsync (CancellationToken cancellationToken = default)
        {
            var manager = Engine.GetService<ICharacterManager>();
            await UniTask.WhenAll(manager.GetAllActors().Select(a => a.ChangeVisibilityAsync(false, Duration, cancellationToken: cancellationToken)));
            if (cancellationToken.CancelASAP) return;
            if (Remove) manager.RemoveAllActors();
        }
    }
}
