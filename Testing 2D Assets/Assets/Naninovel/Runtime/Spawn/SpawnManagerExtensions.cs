// Copyright 2017-2021 Elringus (Artyom Sovetnikov). All rights reserved.

using System.Collections.Generic;
using UniRx.Async;

namespace Naninovel
{
    /// <summary>
    /// Provides extension methods for <see cref="ISpawnManager"/>.
    /// </summary>
    public static class SpawnManagerExtensions
    {
        /// <summary>
        /// Spawns a new object with the provided path or returns one if it's already spawned.
        /// </summary>
        public static async UniTask<SpawnedObject> GetOrSpawnAsync (this ISpawnManager manager, string path,
            CancellationToken cancellationToken = default)
        {
            return manager.IsSpawned(path)
                ? manager.GetSpawned(path)
                : await manager.SpawnAsync(path, cancellationToken);
        }

        /// <summary>
        /// Spawns an object with the provided path and applies provided spawn parameters.
        /// </summary>
        public static async UniTask<SpawnedObject> SpawnWithParametersAsync (this ISpawnManager manager, string path,
            IReadOnlyList<string> parameters, CancellationToken cancellationToken = default)
        {
            var spawnedObject = await manager.SpawnAsync(path, cancellationToken);
            if (cancellationToken.CancelASAP) return spawnedObject;
            spawnedObject.SetSpawnParameters(parameters);
            return spawnedObject;
        }

        /// <summary>
        /// Spawns an object with the provided path, applies provided spawn parameters and waits.
        /// </summary>
        public static async UniTask<SpawnedObject> SpawnWithParametersAndWaitAsync (this ISpawnManager manager, string path,
            IReadOnlyList<string> parameters, CancellationToken cancellationToken = default)
        {
            var spawnedObject = await manager.SpawnWithParametersAsync(path, parameters, cancellationToken);
            if (cancellationToken.CancelASAP) return spawnedObject;
            await spawnedObject.AwaitSpawnAsync(cancellationToken);
            return spawnedObject;
        }
    }
}
