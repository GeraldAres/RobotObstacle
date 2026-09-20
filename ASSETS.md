# 3D Visualization Assets

This project uses a procedural Python-generated low-poly robot and obstacle rather than downloading an external asset. That keeps the project self-contained and avoids uncertain third-party licensing issues.

## Procedural model
- Name: Low-poly robot + obstacle scene
- Source: Generated in Python with PyVista
- License: Procedurally generated for this educational project; no external model license constraints apply
- Author: This project
- Attribution: Not required for the generated model

## Separation of responsibilities
- C#: canonical fuzzy logic and motion decision
- Python: visualization only
- The Python code does not contain fuzzy membership functions, rules, or defuzzification logic.
