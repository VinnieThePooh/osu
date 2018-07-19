﻿// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics;
using System;

namespace osu.Game.Screens.Edit.Screens.Setup.Components
{
    public class OsuColourPicker : Container, IHasCurrentValue<Color4>
    {
        private readonly OsuSetupCircularButton copyButton;
        private readonly OsuSetupCircularButton pasteButton;
        private readonly FillFlowContainer buttonContainer;
        private readonly OsuSetupTextBox colourText;
        private readonly Box colourInfoBackground;
        private readonly FillFlowContainer colourInfoContainer;
        private readonly OsuColourPickerGradient colourPickerGradient;
        private readonly OsuColourPickerHue colourPickerHue;

        private bool isColourChangedFromGradient;

        public const float SIZE_X = 200;
        public const float SIZE_Y = 330;
        public const float COLOUR_INFO_HEIGHT = 30;
        public const float DEFAULT_PADDING = 10;

        private float leftPadding => OsuSetupColourButton.SIZE_Y / 2 + DEFAULT_PADDING * 2;

        public Anchor Origin
        {
            get => base.Origin;
            set
            {
                if (value == base.Origin)
                    return;

                if (value.HasFlag(Anchor.x1) || value.HasFlag(Anchor.y1))
                    throw new InvalidOperationException("Cannot set colour picker origin to centre in any axis.");

                base.Origin = value;
                updateOrigin();
            }
        }

        public event Action<Color4> ColourChanged;

        public void TriggerColourChanged(Color4 newValue)
        {
            ColourChanged?.Invoke(newValue);
        }

        public OsuColourPicker()
        {
            Size = new Vector2(0, OsuSetupColourButton.SIZE_Y);
            CornerRadius = 10;
            Masking = true;

            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = OsuColour.FromHex("232e34"),
                },
                colourInfoBackground = new Box
                {
                    Size = new Vector2(SIZE_X, 75),
                    Colour = OsuColour.FromHex("1c2125"),
                },
                colourInfoContainer = new FillFlowContainer
                {
                    Direction = FillDirection.Vertical,
                    Spacing = new Vector2(5),
                    Children = new Drawable[]
                    {
                        new Container
                        {
                            Size = new Vector2(SIZE_X, COLOUR_INFO_HEIGHT + DEFAULT_PADDING),
                            Children = new Drawable[]
                            {
                                colourText = new OsuSetupTextBox
                                {
                                    Anchor = Anchor.TopRight,
                                    Origin = Anchor.TopRight,
                                    Size = new Vector2(SIZE_X - leftPadding, COLOUR_INFO_HEIGHT),
                                    Position = new Vector2(-DEFAULT_PADDING, DEFAULT_PADDING),
                                    CornerRadius = 10,
                                    Text = "#ffffff",
                                },
                            }
                        },
                        new Container
                        {
                            Size = new Vector2(SIZE_X, SIZE_Y - COLOUR_INFO_HEIGHT - DEFAULT_PADDING),
                            Children = new Drawable[]
                            {
                                buttonContainer = new FillFlowContainer
                                {
                                    Direction = FillDirection.Horizontal,
                                    Spacing = new Vector2(10),
                                    Padding = new MarginPadding { Left = leftPadding - 10, Right = 15 },
                                    Children = new[]
                                    {
                                        copyButton = new OsuSetupCircularButton
                                        {
                                            Anchor = Anchor.TopLeft,
                                            Origin = Anchor.TopLeft,
                                            Size = new Vector2((SIZE_X - leftPadding - 10) / 2, 20),
                                            CornerRadius = 10,
                                            LabelText = "Copy",
                                        },
                                        pasteButton = new OsuSetupCircularButton
                                        {
                                            Anchor = Anchor.TopLeft,
                                            Origin = Anchor.TopLeft,
                                            Size = new Vector2((SIZE_X - leftPadding - 10) / 2, 20),
                                            CornerRadius = 10,
                                            LabelText = "Paste",
                                        }
                                    }
                                }
                            }
                        },
                    }
                },
                colourPickerGradient = new OsuColourPickerGradient
                {
                    Position = new Vector2(10, 85),
                    ActiveColour = Color4.Red
                },
                colourPickerHue = new OsuColourPickerHue
                {
                    Position = new Vector2(10, 275),
                }
            };

            Current.Value = Color4.White;
            Current.ValueChanged += newValue =>
            {
                if (!isColourChangedFromGradient)
                {
                    colourPickerGradient.Current.Value = newValue;
                    colourPickerHue.Hue = Color4.ToHsv(newValue).X;
                }
                colourText.Text = toHexRGBString(newValue);
                TriggerColourChanged(newValue);
            };

            colourPickerHue.HueChanged += a => colourPickerGradient.ActiveColour = a;
            colourPickerGradient.SelectedColourChanged += a =>
            {
                isColourChangedFromGradient = true;
                Current.Value = a;
                isColourChangedFromGradient = false;
            };
            
            colourText.OnCommit += delegate
            {
                try
                {
                    Current.Value = OsuColour.FromHex(colourText.Text.Substring(1, 6));
                }
                catch
                {
                    colourText.Text = toHexRGBString(Current.Value);
                }
            };
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour osuColour)
        {
            copyButton.DefaultColour = osuColour.Blue;
            pasteButton.DefaultColour = osuColour.Blue;
        }

        public void Expand()
        {
            this.ResizeWidthTo(SIZE_X, 500, Easing.OutQuint);
            this.ResizeHeightTo(SIZE_Y, 500, Easing.OutQuint);
        }

        public void Collapse()
        {
            this.ResizeHeightTo(OsuSetupColourButton.SIZE_Y, 500, Easing.OutQuint);
            this.ResizeWidthTo(0, 500, Easing.OutQuint);
        }

        private void updateOrigin()
        {
            if (Origin.HasFlag(Anchor.x2))
            {
                buttonContainer.Padding = new MarginPadding { Left = 10, Right = leftPadding - 10 };
                colourText.X = DEFAULT_PADDING;
                colourText.Anchor = Anchor.TopLeft;
                colourText.Origin = Anchor.TopLeft;
            }
            else if (Origin.HasFlag(Anchor.x0))
            {
                buttonContainer.Padding = new MarginPadding { Left = leftPadding - 10, Right = 15 };
                colourText.X = -DEFAULT_PADDING;
                colourText.Anchor = Anchor.TopRight;
                colourText.Origin = Anchor.TopRight;
            }
            if (Origin.HasFlag(Anchor.y0))
            {
                Y = 0;
                colourPickerGradient.Y = 85;
                colourPickerHue.Y = 275;
                colourInfoContainer.Y = 0;
                colourInfoBackground.Anchor = Anchor.TopLeft;
                colourInfoBackground.Origin = Anchor.TopLeft;
            }
            else if (Origin.HasFlag(Anchor.y2))
            {
                Y = OsuSetupColourButton.SIZE_Y;
                colourPickerGradient.Y = 10;
                colourPickerHue.Y = 200;
                colourInfoContainer.Y = SIZE_Y - OsuSetupColourButton.SIZE_Y;
                colourInfoBackground.Anchor = Anchor.BottomLeft;
                colourInfoBackground.Origin = Anchor.BottomLeft;
            }
        }

        public Bindable<Color4> Current { get; } = new Bindable<Color4>();

        private string toHexRGBString(Color4 colour) => $"#{((byte)(colour.R * 255)).ToString("X2").ToLower()}{((byte)(colour.G * 255)).ToString("X2").ToLower()}{((byte)(colour.B * 255)).ToString("X2").ToLower()}";
    }
}
