import argparse
import json
import os
import sys
import time

import numpy as np
import pyvista as pv


STYLE_COLORS = {
    "robot": (0.2, 0.7, 1.0),
    "obstacle": (0.9, 0.4, 0.2),
    "ground": (0.15, 0.25, 0.2),
    "sensor": (1.0, 0.96, 0.5),
}


def build_robot(center=(0.0, 0.0, 0.5), heading=0.0):
    body = pv.Cylinder(radius=0.8, height=0.8, direction=(0, 0, 1), center=(center[0], center[1], center[2] + 0.4))
    wheel1 = pv.Cylinder(radius=0.22, height=0.2, direction=(0, 1, 0), center=(center[0] - 0.45, center[1], center[2] - 0.2))
    wheel2 = pv.Cylinder(radius=0.22, height=0.2, direction=(0, 1, 0), center=(center[0] + 0.45, center[1], center[2] - 0.2))
    wheel3 = pv.Cylinder(radius=0.22, height=0.2, direction=(0, 1, 0), center=(center[0], center[1] - 0.45, center[2] - 0.2))
    wheel4 = pv.Cylinder(radius=0.22, height=0.2, direction=(0, 1, 0), center=(center[0], center[1] + 0.45, center[2] - 0.2))
    body = body.merge([wheel1, wheel2, wheel3, wheel4])
    body.rotate_z(heading, point=(0, 0, 0), inplace=True)
    body.translate((center[0], center[1], 0.0), inplace=True)
    return body


def build_obstacle(position=(5.0, 0.0, 0.5), scale=(1.2, 1.2, 1.0)):
    box = pv.Box(bounds=(-scale[0] / 2, scale[0] / 2, -scale[1] / 2, scale[1] / 2, 0.0, scale[2]))
    box.translate((position[0], position[1], 0.0), inplace=True)
    return box


def clamp(value, lower, upper):
    return max(lower, min(upper, value))


def load_state(path):
    if not os.path.exists(path):
        return None

    try:
        with open(path, 'r', encoding='utf-8') as handle:
            payload = json.load(handle)
        return payload
    except Exception:
        return None


def main():
    parser = argparse.ArgumentParser(description='Robot obstacle visualization')
    parser.add_argument('--state-file', default='robot_visualization_state.json')
    args = parser.parse_args()

    plotter = pv.Plotter(window_size=(1100, 800), off_screen=False)
    plotter.set_background('white')
    plotter.camera.position = (10, -12, 8)
    plotter.camera.focal_point = (0, 0, 0)
    plotter.camera.up = (0, 0, 1)

    ground = pv.Plane(center=(0, 0, -0.05), direction=(0, 0, 1), i_size=20, j_size=20)
    ground = ground.triangulate()
    plotter.add_mesh(ground, color=STYLE_COLORS['ground'], smooth_shading=True, opacity=0.9)

    obstacle = build_obstacle(position=(5.0, 0.0, 0.5), scale=(1.6, 1.6, 1.0))
    plotter.add_mesh(obstacle, color=STYLE_COLORS['obstacle'], opacity=0.9)

    robot_mesh = build_robot(center=(0.0, 0.0, 0.5), heading=0.0)
    plotter.add_mesh(robot_mesh, color=STYLE_COLORS['robot'], smooth_shading=True)

    sensor_line = pv.Line((0.0, 0.0, 0.8), (8.0, 0.0, 0.8))
    sensor_actor = plotter.add_mesh(sensor_line, color=STYLE_COLORS['sensor'], line_width=3)

    labels = []
    labels.append(plotter.add_text('Robot Obstacle Avoidance', position=(10, 20), font_size=18, color='black'))
    labels.append(plotter.add_text('Distance / Direction from C# Mamdani output', position=(10, 40), font_size=12, color='darkgray'))

    last_state = None
    while True:
        state = load_state(args.state_file)
        if state is not None:
            dist = float(state.get('distance', 50.0))
            direction = float(state.get('direction', 0.0))
            crisp = float(state.get('crisp_output', 50.0))
            movement = str(state.get('movement', 'FORWARD')).upper()

            obstacle_x = 5.0 + (100.0 - dist) * 0.08
            obstacle_y = clamp(direction * 0.14, -6.0, 6.0)
            obstacle = build_obstacle(position=(obstacle_x, obstacle_y, 0.5), scale=(1.6, 1.6, 1.0))
            plotter.add_mesh(obstacle, color=STYLE_COLORS['obstacle'], opacity=0.9)

            robot_heading = np.deg2rad(-direction)
            robot_x = np.cos(robot_heading) * 1.0
            robot_y = np.sin(robot_heading) * 1.0
            robot_mesh = build_robot(center=(0.0, 0.0, 0.5), heading=np.degrees(robot_heading))
            plotter.clear_actors()
            plotter.add_mesh(ground, color=STYLE_COLORS['ground'], smooth_shading=True, opacity=0.9)
            plotter.add_mesh(robot_mesh, color=STYLE_COLORS['robot'], smooth_shading=True)
            plotter.add_mesh(obstacle, color=STYLE_COLORS['obstacle'], opacity=0.9)

            sensor_end = (obstacle_x, obstacle_y, 0.8)
            sensor_line = pv.Line((0.0, 0.0, 0.8), sensor_end)
            plotter.add_mesh(sensor_line, color=STYLE_COLORS['sensor'], line_width=3)

            plotter.camera.position = (12.0, -10.0, 7.0)
            plotter.camera.focal_point = (0.0, 0.0, 0.0)
            plotter.camera.up = (0, 0, 1)
            plotter.add_text(f'Distance {dist:0.2f} cm', position=(10, 20), font_size=18, color='black')
            plotter.add_text(f'Direction {direction:0.2f}', position=(10, 42), font_size=16, color='darkgray')
            plotter.add_text(f'Crisp {crisp:0.2f}  |  {movement}', position=(10, 62), font_size=16, color='black')
            plotter.show(auto_close=False)
            time.sleep(0.2)
            last_state = state

        time.sleep(0.1)


if __name__ == '__main__':
    try:
        main()
    except KeyboardInterrupt:
        sys.exit(0)
