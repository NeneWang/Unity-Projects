// Copyright 2017-2021 Elringus (Artyom Sovetnikov). All rights reserved.

using System.Collections.Generic;
using System.Linq;
using UniRx.Async;

namespace Naninovel.Commands
{
    /// <summary>
    /// Hides (removes) actors (character, background, text printer, choice handler) with the specified IDs.
    /// In case multiple actors with the same ID found (eg, a character and a printer), will affect only the first found one.
    /// </summary>
    [CommandAlias("hide")]
    public class HideActors : Command
    {
        /// <summary>
        /// IDs of the actors to hide.
        /// </summary>
        [ParameterAlias(NamelessParameterAlias), RequiredParameter, IDEActor]
        public StringListParameter ActorIds;
        /// <summary>
        /// Duration (in seconds) of the fade animation. Default value: 0.35 seconds.
        /// </summary>
        [ParameterAlias("time"), ParameterDefaultValue("0.35")]
        public DecimalParameter Duration = .35f;
        /// <summary>
        /// Whether to remove (destroy) the actor after it's hidden.
        /// Use to unload resources associated with the actor and prevent memory leaks.
        /// </summary>
        [ParameterDefaultValue("false")]
        public BooleanParameter Remove = false;

        public override async UniTask ExecuteAsync (CancellationToken cancellationToken = default)
        {
            var managers = Engine.FindAllServices<IActorManager>(c => ActorIds.Any(id => c.ActorExists(id)));
            var tasks = new List<UniTask>();
            foreach (var actorId in ActorIds)
                if (managers.FirstOrDefault(m => m.ActorExists(actorId)) is IActorManager manager)
                    tasks.Add(HideInManager(actorId, manager, cancellationToken));
                else LogErrorWithPosition($"Failed to hide `{actorId}` actor: can't find any managers with `{actorId}` actor.");
            await UniTask.WhenAll(tasks);
        }

        private async UniTask HideInManager (string actorId, IActorManager manager, CancellationToken cancellationToken)
        {
            var actor = manager.GetActor(actorId);
            await actor.ChangeVisibilityAsync(false, Duration, cancellationToken: cancellationToken);
            if (cancellationToken.CancelASAP) return;
            if (Remove) manager.RemoveActor(actorId);
        }
    }
}
