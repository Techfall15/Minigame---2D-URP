# Minigame - 2D URP

Recent Changes:
================================
- Created a 10x10 grid in the inspector. The user can enter 2 points <i>(x1,y1) & (x2,y2)</i>. The grid then shades in the area <i>above/below</i> the line that represents the area created by the entered information.<br>

  <i>NOTE</i>
  ----------------------
  I plan to use this to limit the areas that stars are able to spawn when enabled, and have an easily identifiable way to reflect what to expect with the given information.



To-Do:
--------------------------------
(_) Turn the Vector2 fields for the line's points, into a slider.
-  ( Could also just find a way to limit the values between (-5f, 5f) )

(_) Research drawing gizmos in the inspector.
-  Plan to use this to draw a line between the points. As well as create actual points on the grid.
-  Ultimately the user should be able to click, drag, drop, the two points to on the grid.

(_) Change the color of the grid.

(_) Change the line's point color.

(_) Limit star system spawning to the area displayed in the grid.

(_) Create functions for toggling spawning above/below the line.

(_) Create labels displaying the slope and y-intercept of the line created.
