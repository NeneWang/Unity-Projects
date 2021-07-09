// Copyright 2017-2021 Elringus (Artyom Sovetnikov). All rights reserved.

using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

namespace Naninovel.UI
{
    /// <inheritdoc cref="IMovieUI"/>
    public class MovieUI : CustomUI, IMovieUI
    {
        protected virtual RawImage MovieImage => movieImage;
        protected virtual RawImage FadeImage => fadeImage;

        [SerializeField] private RawImage movieImage = default;
        [SerializeField] private RawImage fadeImage = default;

        private IMoviePlayer moviePlayer;

        protected override void Awake ()
        {
            base.Awake();

            this.AssertRequiredObjects(MovieImage, FadeImage);
            moviePlayer = Engine.GetService<IMoviePlayer>();
        }

        protected override void OnEnable ()
        {
            base.OnEnable();

            moviePlayer.OnMoviePlay += HandleMoviePlay;
            moviePlayer.OnMovieStop += HandleMovieStop;
            moviePlayer.OnMovieTextureReady += HandleMovieTextureReady;
        }

        protected override void OnDisable ()
        {
            base.OnDisable();

            moviePlayer.OnMoviePlay -= HandleMoviePlay;
            moviePlayer.OnMovieStop -= HandleMovieStop;
            moviePlayer.OnMovieTextureReady -= HandleMovieTextureReady;
        }

        protected virtual void HandleMoviePlay ()
        {
            FadeImage.texture = moviePlayer.FadeTexture;
            MovieImage.SetOpacity(0);
            ChangeVisibilityAsync(true, moviePlayer.Configuration.FadeDuration).Forget();
        }

        protected virtual void HandleMovieTextureReady (Texture texture)
        {
            MovieImage.texture = texture;
            MovieImage.SetOpacity(1);
        }

        protected virtual async void HandleMovieStop ()
        {
            MovieImage.SetOpacity(0);
            await ChangeVisibilityAsync(false, moviePlayer.Configuration.FadeDuration);
        }
    }
}
