# Robot Obstacle Avoidance Using Mamdani Fuzzy Logic

This project implements a Mamdani fuzzy logic controller (FLC) for a robot obstacle-avoidance task. The application is a C# WinForms simulation that demonstrates fuzzification, rule evaluation, Mamdani implication (clipping), aggregation (max), and centroid defuzzification.

## 1. Problem Statement
The robot must decide how to move when an obstacle is detected. Inputs:
- Distance from obstacle (0..100 cm)
- Direction of obstacle (-100 .. 100, where -100 = far left, 0 = center, 100 = far right)

Outputs (movement):
- Turn Left
- Forward
- Turn Right
- Stop

The controller produces a numeric output (0..100) which is converted to a movement label.

## 2. Why Fuzzy Logic?
Obstacle avoidance often requires smooth transitions rather than abrupt changes at arbitrary thresholds. For example, a distance of 49 cm and 51 cm should not produce entirely different behaviors. Fuzzy logic allows partial membership (e.g., Near = 0.6, Medium = 0.4) so multiple rules can contribute to the final decision and produce smoother, more human-like control.

Fuzzy logic is preferable for this simulation when compared to hard-coded if/else because it naturally handles vagueness and continuous control and better represents the notion of "partially near" or "slightly to the left".

## 3. System Inputs and Output
Distance
- Variable: Distance
- Unit: centimeters
- Universe: 0 – 100
- Linguistic terms: Near, Medium, Far
- MF type: Triangular / shoulder triangles

Direction
- Variable: Direction
- Unit: normalized directional position
- Universe: -100 – 100
- Linguistic terms: Left, Center, Right
- MF type: Triangular / shoulder triangles
- Notes: -100 = far left, 0 = center, 100 = far right

Movement (output)
- Numeric universe used for centroid defuzzification
- 0   = Turn Left
- 33  = Forward
- 66  = Turn Right
- 100 = Stop

The numeric output is not a distance or speed but a position on a movement-control axis so the centroid can be computed.

## 4. Membership Functions (parameters)
Distance
- Near   = triangle(0, 0, 50)
- Medium = triangle(20, 50, 80)
- Far    = triangle(50, 100, 100)

Direction
- Left   = triangle(-100, -100, 0)
- Center = triangle(-50, 0, 50)
- Right  = triangle(0, 100, 100)

Movement (output)
- Turn Left  = triangle(0, 0, 33)
- Forward    = triangle(0, 33, 66)
- Turn Right = triangle(33, 66, 100)
- Stop       = triangle(66, 100, 100)

## 5. Rule Base (3×3 matrix)
Distance\Direction | Left       | Center   | Right
-------------------|------------|----------|--------
Near               | Turn Right | Stop     | Turn Left
Medium             | Turn Right | Forward  | Turn Left
Far                | Forward    | Forward  | Forward

Reasoning for each rule is documented in DEFENSE.md.

## 6. Mamdani Inference Process
Steps implemented exactly:
1. Fuzzification – convert crisp inputs into μ values for each MF.
2. Rule evaluation – AND implemented with Math.Min.
3. Mamdani implication – each rule clips its consequent MF with Math.Min(ruleStrength, μ_output(y)).
4. Aggregation – combine clipped outputs with Math.Max.
5. Defuzzification – centroid: Σ(y·μ(y)) / Σ μ(y). Numerical integration uses a step of 0.5.

If no rules fire (Σ μ(y) == 0), the UI displays "NO ACTIVATION" and does not map the undefined centroid to a movement label.

## 7. UI Demonstration
- Inputs: TrackBars and NumericUpDowns for Distance and Direction.
- Fuzzification: numeric display of membership degrees.
- Rule evaluation: textual display of 9 rule firing strengths.
- Output: Aggregated fuzzy output, numeric centroid, and movement label.
- Visualizations: Membership plots for Distance, Direction, and Output plus a control-surface heatmap (Distance vs Direction -> Crisp Output).

## 8. Tests
A test harness is included in the app (Run Tests) which executes the nine canonical cases and several boundary points and prints membership values, rule activations, centroid, and movement.

## 9. Limitations
- The output numeric encoding (0,33,66,100) is a design choice to allow centroid defuzzification; that mapping is documented and defended in DEFENSE.md.
- A 2D heatmap is used for the control surface. A full 3D interactive surface is not included to keep the project dependency-free and simple.

## 10. How to run
Open the solution in Visual Studio (targets .NET Framework 4.8) and run the project. Use the TrackBars or numeric inputs to change Distance and Direction; the system updates dynamically. Use Run Tests to verify canonical scenarios.
