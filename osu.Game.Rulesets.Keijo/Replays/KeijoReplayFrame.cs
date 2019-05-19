﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Beatmaps;
using osu.Game.Replays.Legacy;
using osu.Game.Rulesets.Replays;
using osu.Game.Rulesets.Replays.Types;
using osuTK;

namespace osu.Game.Rulesets.Keijo.Replays
{
    public class KeijoReplayFrame : ReplayFrame, IConvertibleReplayFrame
    {
        public Vector2 Position;
        public List<KeijoAction> Actions = new List<KeijoAction>();

        public KeijoReplayFrame()
        {
        }

        public KeijoReplayFrame(double time, Vector2 position, params KeijoAction[] actions)
            : base(time)
        {
            Position = position;
            Actions.AddRange(actions);
        }

        public void ConvertFrom(LegacyReplayFrame legacyFrame, IBeatmap beatmap)
        {
            Position = legacyFrame.Position;
            if (legacyFrame.MouseLeft) Actions.Add(KeijoAction.LeftButton);
            if (legacyFrame.MouseRight) Actions.Add(KeijoAction.RightButton);
        }
    }
}
