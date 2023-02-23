using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using PrimeInputActions;
using ProTrans;
using UniInject;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

// Disable warning about fields that are never assigned, their values are injected.
#pragma warning disable CS0649

public class DesignOptionsControl : AbstractOptionsSceneControl, INeedInjection, ITranslator
{
    [Inject]
    private SceneNavigator sceneNavigator;

    [Inject]
    private TranslationManager translationManager;

    [Inject]
    private ThemeManager themeManager;

    [Inject(UxmlName = R.UxmlNames.themeContainer)]
    private VisualElement themeContainer;

    [Inject(UxmlName = R.UxmlNames.noteDisplayModeContainer)]
    private VisualElement noteDisplayModeContainer;

    [Inject(UxmlName = R.UxmlNames.lyricsOnNotesContainer)]
    private VisualElement lyricsOnNotesContainer;

    [Inject(UxmlName = R.UxmlNames.staticLyricsContainer)]
    private VisualElement staticLyricsContainer;

    [Inject(UxmlName = R.UxmlNames.pitchIndicatorContainer)]
    private VisualElement pitchIndicatorContainer;

    [Inject(UxmlName = R.UxmlNames.imageAsCursorContainer)]
    private VisualElement imageAsCursorContainer;

    [Inject(UxmlName = R.UxmlNames.animateSceneChangeContainer)]
    private VisualElement animateSceneChangeContainer;

    [Inject]
    private Settings settings;

    [Inject]
    private UiManager uiManager;

    protected override void Start()
    {
        base.Start();
        
        new NoteDisplayModeItemPickerControl(noteDisplayModeContainer.Q<ItemPicker>())
            .Bind(() => settings.GraphicSettings.noteDisplayMode,
                newValue => settings.GraphicSettings.noteDisplayMode = newValue);

        new BoolPickerControl(lyricsOnNotesContainer.Q<ItemPicker>())
            .Bind(() => settings.GraphicSettings.showLyricsOnNotes,
                newValue => settings.GraphicSettings.showLyricsOnNotes = newValue);

        new BoolPickerControl(staticLyricsContainer.Q<ItemPicker>())
            .Bind(() => settings.GraphicSettings.showStaticLyrics,
                newValue => settings.GraphicSettings.showStaticLyrics = newValue);

        new BoolPickerControl(pitchIndicatorContainer.Q<ItemPicker>())
            .Bind(() => settings.GraphicSettings.showPitchIndicator,
                newValue => settings.GraphicSettings.showPitchIndicator = newValue);

        new BoolPickerControl(imageAsCursorContainer.Q<ItemPicker>())
            .Bind(() => settings.GraphicSettings.useImageAsCursor,
                newValue => settings.GraphicSettings.useImageAsCursor = newValue);

        new BoolPickerControl(animateSceneChangeContainer.Q<ItemPicker>())
            .Bind(() => settings.GraphicSettings.AnimateSceneChange,
                newValue => settings.GraphicSettings.AnimateSceneChange = newValue);

        // Load available themes:
        List<ThemeMeta> themeMetas = themeManager.GetThemeMetas();
        LabeledItemPickerControl<ThemeMeta> themePickerControl = new(themeContainer.Q<ItemPicker>(), themeMetas);
        themePickerControl.GetLabelTextFunction = themeMeta => ThemeMetaUtils.GetDisplayName(themeMeta);
        themePickerControl.Bind(
            () => themeManager.GetCurrentTheme(),
            newValue => themeManager.SetCurrentTheme(newValue));
    }

    public void UpdateTranslation()
    {
        themeContainer.Q<Label>().text = TranslationManager.GetTranslation(R.Messages.options_design_theme);
        noteDisplayModeContainer.Q<Label>().text = TranslationManager.GetTranslation(R.Messages.options_noteDisplayMode);
        staticLyricsContainer.Q<Label>().text = TranslationManager.GetTranslation(R.Messages.options_showStaticLyrics);
        lyricsOnNotesContainer.Q<Label>().text = TranslationManager.GetTranslation(R.Messages.options_showLyricsOnNotes);
        pitchIndicatorContainer.Q<Label>().text = TranslationManager.GetTranslation(R.Messages.options_showPitchIndicator);
        imageAsCursorContainer.Q<Label>().text = TranslationManager.GetTranslation(R.Messages.options_useImageAsCursor);
    }

    public override bool HasHelpDialog => true;
    public override MessageDialogControl CreateHelpDialogControl()
    {
        Dictionary<string, string> titleToContentMap = new()
        {
            { TranslationManager.GetTranslation(R.Messages.options_design_helpDialog_customThemes_title),
                TranslationManager.GetTranslation(R.Messages.options_design_helpDialog_customThemes,
                    "path", ApplicationUtils.ReplacePathsWithDisplayString(ThemeManager.GetAbsoluteUserDefinedThemesFolder())) },
        };
         MessageDialogControl helpDialogControl = uiManager.CreateHelpDialogControl(
            TranslationManager.GetTranslation(R.Messages.options_design_helpDialog_title),
            titleToContentMap);
        helpDialogControl.AddButton(TranslationManager.GetTranslation(R.Messages.viewMore),
            () => Application.OpenURL(TranslationManager.GetTranslation(R.Messages.uri_howToAddCustomThemes)));
        helpDialogControl.AddButton("Themes Folder",
            () => ApplicationUtils.OpenDirectory(ThemeManager.GetAbsoluteUserDefinedThemesFolder()));
        return helpDialogControl;
    }
}
