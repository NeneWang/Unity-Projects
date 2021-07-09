// Copyright 2017-2021 Elringus (Artyom Sovetnikov). All rights reserved.

using UniRx.Async;

namespace Naninovel.Commands
{
    /// <summary>
    /// Adds a line break to a text printer.
    /// </summary>
    /// <remarks>
    /// Consider using `&lt;br&gt;` tag instead with [TMPro printers](/guide/text-printers.md#textmesh-pro).
    /// </remarks>
    [CommandAlias("br")]
    public class AppendLineBreak : PrinterCommand
    {
        /// <summary>
        /// Number of line breaks to add.
        /// </summary>
        [ParameterAlias(NamelessParameterAlias), ParameterDefaultValue("1")]
        public IntegerParameter Count = 1;
        /// <summary>
        /// ID of the printer actor to use. Will use a default one when not provided.
        /// </summary>
        [ParameterAlias("printer"), IDEActor(TextPrintersConfiguration.DefaultPathPrefix)]
        public StringParameter PrinterId;
        /// <summary>
        /// ID of the actor, which should be associated with the appended line break.
        /// </summary>
        [ParameterAlias("author"), IDEActor(CharactersConfiguration.DefaultPathPrefix)]
        public StringParameter AuthorId;

        protected override string AssignedPrinterId => PrinterId;
        protected override string AssignedAuthorId => AuthorId;
        protected IUIManager UIManager => Engine.GetService<IUIManager>();

        public override async UniTask ExecuteAsync (CancellationToken cancellationToken = default)
        {
            var printer = await GetOrAddPrinterAsync();
            if (cancellationToken.CancelASAP) return;
            var backlogUI = UIManager.GetUI<UI.IBacklogUI>();

            var breaks = string.Empty;
            for (int i = 0; i < Count; i++)
                breaks += "\n";
            printer.Text += breaks;
            backlogUI?.AppendMessage(breaks);
        }
    }
}
