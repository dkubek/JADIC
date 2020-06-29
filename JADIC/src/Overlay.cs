﻿using System.Drawing;

public class Overlay : IDrawable
{
    const int COOLDOWN_METER_WIDTH = 300;
    const int COOLDOWN_METER_HEIGHT = 10;
    const int EDGE_OFFSET = 25;
    const int MOVE_JITTER = 2;
    const int HEART_SIZE = 50;
    const int HEART_PADDING = 10;

    private World world;
    public Overlay(World world)
    {
        this.world = world;
    }
    public void Update()
    {
    }

    public void Render(Size resolution, Graphics container)
    {
        int dx = world.MainPlayer.HorizontalSign * MOVE_JITTER;
        int dy = world.MainPlayer.VerticalSign * MOVE_JITTER;

        int offsetX = EDGE_OFFSET - dx;
        int offsetY = EDGE_OFFSET - dy;

        RenderBorder(
            new Point(offsetX, offsetY),
            resolution, container);

        int cooldownMeterX = EDGE_OFFSET + 20 - dx;
        int cooldownMeterY =
            resolution.Height - EDGE_OFFSET - 20 - COOLDOWN_METER_HEIGHT - dy;
        RenderCooldownMeter(
            new Point(cooldownMeterX, cooldownMeterY),
            container
            );

        int heartsX = EDGE_OFFSET + 20 - dx;
        int heartsY = EDGE_OFFSET + 20 - dy;
        RenderHearts(
            new Point(heartsX, heartsY),
            container);

    }

    private void RenderBorder(
        Point baseCoordinates, Size resolution, Graphics container)
    {
        var offsetX = baseCoordinates.X;
        var offsetY = baseCoordinates.Y;

        container.DrawRectangle(
            new Pen(Color.White, 10),
            new Rectangle(
                offsetX,
                offsetY, 
                resolution.Width - 2 * EDGE_OFFSET,
                resolution.Height - 2 * EDGE_OFFSET));
    }

    private void RenderCooldownMeter(
        Point baseCoordinates, Graphics container)
    {
        int meterWidth =
            COOLDOWN_METER_WIDTH -
            world.MainPlayer.projectileCooldownCurrent * COOLDOWN_METER_WIDTH /
            world.MainPlayer.PROJECTILE_COOLDOWN;

        container.FillRectangle(
            new SolidBrush(Color.White),
            new Rectangle(
                baseCoordinates.X, baseCoordinates.Y,
                meterWidth, COOLDOWN_METER_HEIGHT)
            );
    }

    private void RenderHearts(
        Point baseCoordinates, Graphics container)
    {
        for (int i = 0; i < world.MainPlayer.Lives; i++)
        {
            container.DrawImage(
                JADIC.Properties.Resources.heart,
                new Rectangle(
                    baseCoordinates.X + i * (HEART_PADDING + HEART_SIZE), baseCoordinates.Y,
                    HEART_SIZE, HEART_SIZE));
        }
    }
}