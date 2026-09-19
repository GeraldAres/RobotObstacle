DEFENSE: Concise Answers for Live Q&A

What is Fuzzy Logic?
- Fuzzy logic represents partial truth values between 0 and 1. Variables can belong to multiple linguistic sets at once (e.g., "Near" 0.6 and "Medium" 0.4).

Why is Fuzzy Logic appropriate for this problem?
- Obstacle avoidance benefits from smooth transitions and handling of vagueness: distances near boundaries should allow multiple rules to contribute, avoiding abrupt switching that can cause unstable robot behavior.

What is fuzzification?
- Converting crisp inputs (e.g., distance=47 cm) into membership degrees for each fuzzy set using membership functions.

What is a membership function?
- A function μ(x) that maps input x to a membership degree in [0,1]. We use triangular MFs for simplicity and interpretability.

Why triangular MFs?
- They are simple, computationally cheap, and easy to justify visually and analytically. Shoulder triangles are used at boundaries to represent "fully" near or far.

What does membership degree 0.7 mean?
- It means the value belongs to that linguistic set to degree 0.7; rules using that set will be influenced proportionally.

Why use Min for AND?
- The Mamdani t-norm standard choice: Min(a,b) gives the rule firing strength as the weakest antecedent.

What is Mamdani implication and clipping?
- Mamdani implication clips the consequent MF at the rule strength: μ_implicated(y)=Min(ruleStrength, μ_consequent(y)). This constrains the output contribution.

Why use Max for aggregation?
- Max combines the clipped consequents to form an overall output fuzzy set (logical union).

Why centroid defuzzification?
- The centroid yields a balanced numeric output representing the "center of mass" of the aggregated output set. It produces smooth, continuous outputs suitable for control.

Why numeric output values 0,33,66,100?
- These are representative positions on a movement axis: left(0), forward(33), right(66), stop(100). They allow a single numeric universe for centroid calculation. The exact spacing gives Forward a central region that overlaps with left/right.

What happens if no rule fires?
- The app displays "NO ACTIVATION". This avoids mapping a meaningless centroid (e.g., 0) to a movement.

How do boundaries behave?
- At boundaries multiple MFs can be non-zero; multiple rules will contribute, and centroid gives an interpolated output.

Why not use if/else?
- If/else is brittle at thresholds and requires many rules for coverage. Fuzzy rules handle continuous transitions more naturally and with fewer rules.

Why is Forward wide?
- Forward covers the central movement area to prioritize moving forward when obstacles are not dangerously near and/or not strongly to one side. The width balances safety and progress; narrower or wider Forward changes sensitivity and can be tuned.

Why these numeric ranges for inputs?
- Distance 0..100 cm is intuitive and convenient for simulation and units. Direction -100..100 normalizes left-to-right across a symmetric interval.

How do multiple rules affect output?
- Each rule contributes a clipped MF. Aggregation with Max merges these contributions and centroid computes a combined numeric response.

How to demonstrate during demo?
- Move Distance across a boundary (e.g., 49→51) and show membership degrees and resulting centroid shifting smoothly. Show the heatmap to demonstrate global behavior.
