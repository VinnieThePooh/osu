﻿// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Game.Screens.Edit.Screens.Setup;
using osu.Game.Screens.Edit.Screens.Setup.Components;
using osu.Game.Screens.Edit.Screens.Setup.Components.LabelledBoxes;
using osu.Game.Screens.Edit.Screens.Setup.BottomHeaders;
using osu.Game.Screens.Edit.Screens.Setup.Screens;
using osu.Game.Tests.Beatmaps;
using osu.Game.Rulesets;
using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Graphics;

namespace osu.Game.Tests.Visual
{
    [TestFixture]
    public class TestCaseEditorSetupScreen : OsuTestCase
    {
        private Setup setup;

        public override IReadOnlyList<Type> RequiredTypes => new[]
        {
            typeof(Setup),
            typeof(SetupMenuBar),
            typeof(SetupScreenSelectionTabControl),
            typeof(LabelledTextBox),
            typeof(LabelledSwitchButton),
            typeof(LabelledSliderBar),
            typeof(LabelledRadioButtonCollection),
            typeof(OsuSetupTextBox),
            typeof(OsuSetupTickSliderBar),
            typeof(OsuSetupSwitchButton),
            typeof(OsuSetupCircularButton),
            typeof(OsuSetupColourButton),
            typeof(OsuColourPicker),
            typeof(OsuColourPickerGradient),
            typeof(OsuColourPickerHue),
            typeof(NewComboColourButton),
            typeof(Header),
            typeof(GeneralScreen),
            typeof(DifficultyScreen),
            typeof(AudioScreen),
            typeof(ColoursScreen),
            typeof(DesignScreen),
            typeof(AdvancedScreen),
            typeof(GeneralScreenBottomHeader),
            typeof(DifficultyScreenBottomHeader),
        };

        [BackgroundDependencyLoader]
        private void load(OsuGameBase osuGame)
        {
            osuGame.Beatmap.Value = new TestWorkingBeatmap(new RulesetInfo());

            setup = new Setup();
            setup.Beatmap.BindTo(osuGame.Beatmap);
            Child = setup;
            addSteps();
        }

        private void addSteps()
        {
            // ReSharper disable PossibleNullReferenceException
            AddStep("Select General tab", () => setup.MenuBar.Mode.Value = SetupScreenMode.General);
            AddStep("Change artist to \"test artist\"", () => (setup.CurrentScreen as GeneralScreen).ChangeArtist("test artist"));
            AddAssert("Check new artist value", () => setup.CurrentScreen.Beatmap.Value.Metadata.ArtistUnicode == "test artist");
            AddStep("Change romanised artist to \"test romanised artist\"", () => (setup.CurrentScreen as GeneralScreen).ChangeRomanisedArtist("test romanised artist"));
            AddAssert("Check new romanised artist value", () => setup.CurrentScreen.Beatmap.Value.Metadata.Artist == "test romanised artist");
            AddStep("Change title to \"test title\"", () => (setup.CurrentScreen as GeneralScreen).ChangeTitle("test title"));
            AddAssert("Check new title value", () => setup.CurrentScreen.Beatmap.Value.Metadata.TitleUnicode == "test title");
            AddStep("Change romanised title to \"test romanised title\"", () => (setup.CurrentScreen as GeneralScreen).ChangeRomanisedTitle("test romanised title"));
            AddAssert("Check new romanised title value", () => setup.CurrentScreen.Beatmap.Value.Metadata.Title == "test romanised title");
            AddStep("Change difficulty to \"test difficulty\"", () => (setup.CurrentScreen as GeneralScreen).ChangeDifficulty("test difficulty"));
            AddAssert("Check new difficulty value", () => setup.CurrentScreen.Beatmap.Value.Beatmap.BeatmapInfo.Version == "test difficulty");
            AddStep("Change source to \"test source\"", () => (setup.CurrentScreen as GeneralScreen).ChangeSource("test source"));
            AddAssert("Check new source value", () => setup.CurrentScreen.Beatmap.Value.Beatmap.Metadata.Source == "test source");
            AddStep("Change tags to \"test tags\"", () => (setup.CurrentScreen as GeneralScreen).ChangeTags("test tags"));
            AddAssert("Check new tags value", () => setup.CurrentScreen.Beatmap.Value.Beatmap.Metadata.Tags == "test tags");

            AddStep("Select Difficulty tab", () => setup.MenuBar.Mode.Value = SetupScreenMode.Difficulty);
            AddStep("Change drain rate to 7.3", () => (setup.CurrentScreen as DifficultyScreen).ChangeHpDrainRate(7.3f));
            AddAssert("Check new drain rate value", () => setup.CurrentScreen.Beatmap.Value.BeatmapInfo.BaseDifficulty.DrainRate == 7.3f);
            AddStep("Change overall difficulty to 6.8", () => (setup.CurrentScreen as DifficultyScreen).ChangeOverallDifficulty(6.8f));
            AddAssert("Check new overall difficulty value", () => setup.CurrentScreen.Beatmap.Value.BeatmapInfo.BaseDifficulty.OverallDifficulty == 6.8f);
            AddStep("Change approach rate to 7.3", () => (setup.CurrentScreen as DifficultyScreen).ChangeApproachRate(5.9f));
            AddAssert("Check new approach rate value", () => setup.CurrentScreen.Beatmap.Value.BeatmapInfo.BaseDifficulty.ApproachRate == 5.9f);
            AddStep("Change circle size to 4", () => (setup.CurrentScreen as DifficultyScreen).ChangeCircleSize(4));
            AddAssert("Check new circle size value", () => setup.CurrentScreen.Beatmap.Value.BeatmapInfo.BaseDifficulty.CircleSize == 4);

            AddStep("Select Audio tab", () => setup.MenuBar.Mode.Value = SetupScreenMode.Audio);
            AddStep("Reset default sample volume settings", () => (setup.CurrentScreen as AudioScreen).ResetDefaultSampleVolumes());
            AddAssert("Check new default sample volume settings", () =>
            {
                var beatmap = setup.CurrentScreen.Beatmap.Value.Beatmap;
                int defaultVolume = beatmap.ControlPointInfo.SamplePoints.First().SampleVolume;
                foreach (var s in beatmap.ControlPointInfo.SamplePoints)
                    if (s.SampleVolume != defaultVolume)
                        return false;
                return true;
            });
            AddStep("Change default sample bank to soft", () => (setup.CurrentScreen as AudioScreen).ChangeDefaultSampleBank(SampleBank.Soft));
            AddAssert("Check new default sample bank settings", () =>
            {
                var beatmap = setup.CurrentScreen.Beatmap.Value.Beatmap;
                foreach (var s in beatmap.ControlPointInfo.SamplePoints)
                    if (s.SampleBank != "soft")
                        return false;
                return true;
            });
            AddStep("Change default sample bank to drum", () => (setup.CurrentScreen as AudioScreen).ChangeDefaultSampleBank(SampleBank.Drum));
            AddAssert("Check new default sample bank settings", () =>
            {
                var beatmap = setup.CurrentScreen.Beatmap.Value.Beatmap;
                foreach (var s in beatmap.ControlPointInfo.SamplePoints)
                    if (s.SampleBank != "drum")
                        return false;
                return true;
            });
            AddStep("Change default sample bank to normal", () => (setup.CurrentScreen as AudioScreen).ChangeDefaultSampleBank(SampleBank.Normal));
            AddAssert("Check new default sample bank settings", () =>
            {
                var beatmap = setup.CurrentScreen.Beatmap.Value.Beatmap;
                foreach (var s in beatmap.ControlPointInfo.SamplePoints)
                    if (s.SampleBank != "normal")
                        return false;
                return true;
            });
            AddStep("Change default sample volume to 69", () => (setup.CurrentScreen as AudioScreen).ChangeDefaultSampleVolume(69));
            AddAssert("Check new default sample volume settings", () =>
            {
                var beatmap = setup.CurrentScreen.Beatmap.Value.Beatmap;
                foreach (var s in beatmap.ControlPointInfo.SamplePoints)
                    if (s.SampleVolume != 69)
                        return false;
                return true;
            });
            AddStep("Enable samples match playback rate", () => (setup.CurrentScreen as AudioScreen).ChangeSamplesMatchPlaybackRate(true));
            AddAssert("Check new samples match playback rate value", () => setup.CurrentScreen.Beatmap.Value.BeatmapInfo.SamplesMatchPlaybackRate);
            AddStep("Disable samples match playback rate", () => (setup.CurrentScreen as AudioScreen).ChangeSamplesMatchPlaybackRate(false));
            AddAssert("Check new samples match playback rate value", () => !setup.CurrentScreen.Beatmap.Value.BeatmapInfo.SamplesMatchPlaybackRate);

            AddStep("Select Colours tab", () => setup.MenuBar.Mode.Value = SetupScreenMode.Colours);
            AddStep("Change combo colour 1 to #ff0cca", () => (setup.CurrentScreen as ColoursScreen).ChangeComboColour(0, OsuColour.FromHex("ff0cca")));
            AddStep("Change combo colour 2 to #abc143", () => (setup.CurrentScreen as ColoursScreen).ChangeComboColour(1, OsuColour.FromHex("abc143")));
            AddStep("Change combo colour 3 to #def312", () => (setup.CurrentScreen as ColoursScreen).ChangeComboColour(2, OsuColour.FromHex("def312")));
            AddStep("Change combo colour 4 to #123adc", () => (setup.CurrentScreen as ColoursScreen).ChangeComboColour(3, OsuColour.FromHex("123adc")));
            AddStep("Change combo colour 5 to #aabbcc", () => (setup.CurrentScreen as ColoursScreen).ChangeComboColour(4, OsuColour.FromHex("aabbcc")));
            AddStep("Change playfield background colour to #293847", () => (setup.CurrentScreen as ColoursScreen).ChangePlayfieldBackgroundColour(OsuColour.FromHex("293847")));
            AddRepeatStep("Add new combo colours", () => (setup.CurrentScreen as ColoursScreen).AddNewComboColour(), 3);
            AddRepeatStep("Remove combo colours", () => (setup.CurrentScreen as ColoursScreen).RemoveLastComboColour(), 6);

            AddStep("Select Design tab", () => setup.MenuBar.Mode.Value = SetupScreenMode.Design);
            AddStep("Enable enable countdown", () => (setup.CurrentScreen as DesignScreen).ChangeEnableCountdown(true));
            AddAssert("Check new enable countdown value", () => setup.CurrentScreen.Beatmap.Value.BeatmapInfo.Countdown);
            AddStep("Disable enable countdown", () => (setup.CurrentScreen as DesignScreen).ChangeEnableCountdown(false));
            AddAssert("Check new enable countdown value", () => !setup.CurrentScreen.Beatmap.Value.BeatmapInfo.Countdown);
            AddStep("Enable widescreen support", () => (setup.CurrentScreen as DesignScreen).ChangeWidescreenStoryboard(true));
            AddAssert("Check new widescreen support value", () => setup.CurrentScreen.Beatmap.Value.BeatmapInfo.WidescreenStoryboard);
            AddStep("Disable widescreen support", () => (setup.CurrentScreen as DesignScreen).ChangeWidescreenStoryboard(false));
            AddAssert("Check new widescreen support value", () => !setup.CurrentScreen.Beatmap.Value.BeatmapInfo.WidescreenStoryboard);
            AddStep("Enable storyboard in front of combo fire", () => (setup.CurrentScreen as DesignScreen).ChangeStoryFireInFront(true));
            AddAssert("Check new storyboard in front of combo fire value", () => setup.CurrentScreen.Beatmap.Value.BeatmapInfo.StoryFireInFront);
            AddStep("Disable storyboard in front of combo fire", () => (setup.CurrentScreen as DesignScreen).ChangeStoryFireInFront(false));
            AddAssert("Check new storyboard in front of combo fire value", () => !setup.CurrentScreen.Beatmap.Value.BeatmapInfo.StoryFireInFront);
            AddStep("Enable letterbox in breaks", () => (setup.CurrentScreen as DesignScreen).ChangeLetterboxInBreaks(true));
            AddAssert("Check new letterbox in breaks value", () => setup.CurrentScreen.Beatmap.Value.BeatmapInfo.LetterboxInBreaks);
            AddStep("Disable letterbox in breaks", () => (setup.CurrentScreen as DesignScreen).ChangeLetterboxInBreaks(false));
            AddAssert("Check new letterbox in breaks value", () => !setup.CurrentScreen.Beatmap.Value.BeatmapInfo.LetterboxInBreaks);
            AddStep("Enable epilepsy warning", () => (setup.CurrentScreen as DesignScreen).ChangeEpilepsyWarning(true));
            AddAssert("Check new epilepsy warning value", () => setup.CurrentScreen.Beatmap.Value.BeatmapInfo.EpilepsyWarning);
            AddStep("Disable epilepsy warning", () => (setup.CurrentScreen as DesignScreen).ChangeEpilepsyWarning(false));
            AddAssert("Check new epilepsy warning value", () => !setup.CurrentScreen.Beatmap.Value.BeatmapInfo.EpilepsyWarning);

            AddStep("Select Advanced tab", () => setup.MenuBar.Mode.Value = SetupScreenMode.Advanced);
            AddStep("Change stack leniency to 9", () => (setup.CurrentScreen as AdvancedScreen).ChangeStackLeniency(9));
            AddAssert("Check new stack leniency value", () => setup.CurrentScreen.Beatmap.Value.BeatmapInfo.StackLeniency == 9);
            AddStep("Change ruleset to osu!", () => (setup.CurrentScreen as AdvancedScreen).ChangeBeatmapRuleset(0));
            AddAssert("Check new ruleset value", () => setup.CurrentScreen.Beatmap.Value.BeatmapInfo.RulesetID == 0);
            AddStep("Change ruleset to osu!taiko", () => (setup.CurrentScreen as AdvancedScreen).ChangeBeatmapRuleset(1));
            AddAssert("Check new ruleset value", () => setup.CurrentScreen.Beatmap.Value.BeatmapInfo.RulesetID == 1);
            AddStep("Change ruleset to osu!catch", () => (setup.CurrentScreen as AdvancedScreen).ChangeBeatmapRuleset(2));
            AddAssert("Check new ruleset value", () => setup.CurrentScreen.Beatmap.Value.BeatmapInfo.RulesetID == 2);
            AddStep("Change ruleset to osu!mania", () => (setup.CurrentScreen as AdvancedScreen).ChangeBeatmapRuleset(3));
            AddAssert("Check new ruleset value", () => setup.CurrentScreen.Beatmap.Value.BeatmapInfo.RulesetID == 3);
            AddStep("Enable osu!mania special style", () => (setup.CurrentScreen as AdvancedScreen).ChangeManiaSpecialStyle(true));
            AddAssert("Check new special style value", () => setup.CurrentScreen.Beatmap.Value.BeatmapInfo.SpecialStyle);
            AddStep("Disable osu!mania special style", () => (setup.CurrentScreen as AdvancedScreen).ChangeManiaSpecialStyle(false));
            AddAssert("Check new special style value", () => !setup.CurrentScreen.Beatmap.Value.BeatmapInfo.SpecialStyle);
            // ReSharper enable PossibleNullReferenceException
        }
    }
}
