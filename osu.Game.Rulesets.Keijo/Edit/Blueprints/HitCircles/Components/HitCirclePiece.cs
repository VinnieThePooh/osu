﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Graphics;
using osu.Game.Rulesets.Keijo.Objects;
using osu.Game.Rulesets.Keijo.Objects.Drawables.Pieces;
using osuTK;

namespace osu.Game.Rulesets.Keijo.Edit.Blueprints.HitCircles.Components
{
    public class HitCirclePiece : HitObjectPiece
    {
        private readonly HitRegion hitCircle;

        public HitCirclePiece(HitRegion hitCircle)
            : base(hitCircle)
        {
            this.hitCircle = hitCircle;
            Origin = Anchor.Centre;

            Size = new Vector2((float)KeijoHitObject.OBJECT_RADIUS * 2);
            Scale = new Vector2(hitCircle.Scale);
            CornerRadius = Size.X / 2;

            InternalChild = new RingPiece();
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            Colour = colours.Yellow;

            PositionBindable.BindValueChanged(_ => UpdatePosition(), true);
            StackHeightBindable.BindValueChanged(_ => UpdatePosition());
            ScaleBindable.BindValueChanged(scale => Scale = new Vector2(scale.NewValue), true);
        }

        protected virtual void UpdatePosition() => Position = hitCircle.StackedPosition;
    }
}
