﻿// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Screens.Edit.Components;

namespace osu.Game.Screens.Edit.Setup.Components.LabelledComponents
{
    public class LabelledSwitchButton : LabelledComponent, IHasCurrentValue<bool>
    {
        public Bindable<bool> Current { get; set; } = new Bindable<bool>();

        private SwitchButton switchButton;

        [BackgroundDependencyLoader]
        private void load()
        {
            Current.BindTo(switchButton.Current);
        }

        protected override Drawable CreateComponent() => switchButton = new SwitchButton();
    }
}
