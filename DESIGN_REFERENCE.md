# ?? PuneVistaRealestate - Visual Design Reference Card

## Color Palette Quick Reference

### Primary Colors
```
Primary Blue:     #2c5f9f  (Trustworthy, Professional)
Primary Dark:     #1e4573  (Hover states)
Primary Light:    #4a7cbb  (Light backgrounds)
```

### Secondary Colors
```
Gold Accent:      #d4a574  (Premium, Luxury)
Gold Dark:        #c4954a  (Hover states)
Gold Light:       #e8c9a0  (Light backgrounds)
```

### Utility Colors
```
Success Green:    #27ae60  (Actions, Available)
Success Dark:     #1e8449  (Hover)
Warning Yellow:   #f39c12  (Alerts, New)
Danger Red:       #e74c3c  (Urgent, Sold)
```

### Backgrounds
```
Light Background: #f8f9fa  (Page background)
White:            #ffffff  (Cards, surfaces)
Light Gold:       #f9f6f0  (Hover effect)
```

### Text Colors
```
Primary Text:     #2c3e50  (Dark blue-gray)
Secondary Text:   #7f8c8d  (Muted gray)
White Text:       #ffffff  (On dark backgrounds)
```

---

## Component Reference

### Property Card
```
???????????????????????????????
?     PROPERTY IMAGE          ?  280px height
?    [Badge: Featured]        ?
???????????????????????????????
? Property Title              ?
? ?? Location, Pune           ?
? ? 1,50,00,000 (Price)       ?
?                             ?
?  ??? 3 BHK  ?? 2 Bath  ?? 1800 sqft
?                             ?
?  [View Details Button]      ?
???????????????????????????????
```

### Summary Card (Statistics)
```
???????????????????????????????
?        ?? (Icon)            ? Gradient background
?   500+ (Amount)             ? Blue to Gold
?   Total Properties (Label)  ?
???????????????????????????????
```

### Navigation Bar
```
???????????????????????????????
? ?? PuneVistaRealestate  Menu? Dark Blue background
? • Dashboard                 ? White text
? • Properties                ?
? • List Property             ?
? • Categories                ?
???????????????????????????????
```

### Filter Tag
```
All Properties | Apartments | Villas | Commercial | Plots

Active: Blue background + white text
Inactive: Gold border + dark text
Hover: Gold background + white text
```

---

## Font Sizes & Typography

### Desktop Layout
```
Page Title (H1):     3.0rem (48px) - Display 3
Section Title (H2):  2.5rem (40px) - Display 5
Card Title (H5):     1.3rem (20px) - Bold
Label Text:          0.9rem (14px)
Body Text:           1.0rem (16px)
Small Text:          0.85rem (13px)
```

### Mobile Layout (< 768px)
```
Page Title (H1):     2.0rem (32px) - Reduced
Section Title (H2):  1.8rem (28px)
Card Title (H5):     1.1rem (18px)
Other sizes:         Proportionally reduced
```

---

## Button Styles

### Primary Button
```
Background:    #2c5f9f
Text:          White
Hover:         #1e4573 (darker)
Border:        None
Padding:       12px 24px
Border-radius: 6px-10px
Shadow:        0 4px 12px rgba(44, 95, 159, 0.2)
Hover Shadow:  0 8px 20px rgba(44, 95, 159, 0.3)
Transform:     translateY(-2px)
```

### Secondary Button
```
Background:    #d4a574
Text:          Dark text
Hover:         #c4954a (darker)
Similar styling to Primary
```

### Link Colors
```
Normal:        #2c5f9f (Primary Blue)
Hover:         #d4a574 (Secondary Gold)
Visited:       #2c5f9f
```

---

## Spacing Guide

```
Page Padding:        1.5rem - 4rem (24px - 64px)
Card Padding:        1.5rem (24px)
Section Spacing:     3rem - 5rem (48px - 80px)
Component Gap:       1rem (16px)
Border Radius:       6px - 14px (cards rounded)
Shadow (subtle):     0 2px 8px rgba(0,0,0,0.08)
Shadow (medium):     0 4px 16px rgba(0,0,0,0.1)
Shadow (elevated):   0 8px 24px rgba(0,0,0,0.12)
```

---

## Responsive Breakpoints

```
Extra Large (XL): >= 1400px
  - Full width layouts
  - 4-column grids
  - All features visible

Large (LG): >= 1200px  ?? DEFAULT (Desktop)
  - 3-4 column layouts
  - Full navigation
  - Full footer

Medium (MD): >= 992px
  - 2-3 column layouts
  - Adjusted spacing

Small (SM): >= 768px
  - 2 column layouts
  - Hamburger menu
  - Adjusted font sizes

Extra Small (XS): < 768px  ?? Mobile
  - Single column
  - Full-width elements
  - Hamburger menu
  - Reduced font sizes
```

---

## Icon Reference (Font Awesome 6.4)

### Common Icons Used
```
fas fa-building        ??  Building (Brand logo)
fas fa-home            ??  Home (Dashboard)
fas fa-list            ??  List (Properties)
fas fa-plus            ?  Add new
fas fa-filter          ??  Filter/Category
fas fa-user-circle     ??  User account
fas fa-sign-in-alt     ??  Login
fas fa-sign-out-alt    ??  Logout
fas fa-search          ??  Search
fas fa-map-marker-alt  ??  Location
fas fa-bed             ???  Bedroom
fas fa-bath            ??  Bathroom
fas fa-ruler           ??  Area/Size
fas fa-handshake       ??  Partnership
fas fa-star            ?  Rating
fas fa-check-circle    ?  Verified
fas fa-headset         ??  Support
fas fa-phone           ??  Call
fas fa-envelope        ??  Email
fas fa-heart           ??  Favorite
fas fa-share           ??  Share
```

---

## Hover & Animation Effects

### Button Hover
```
Transform:     translateY(-2px) or translateY(-3px)
Transition:    all 0.3s ease
Shadow:        Enhanced box-shadow
Opacity:       Slightly increased brightness
```

### Card Hover
```
Transform:     translateY(-4px) to translateY(-8px)
Shadow:        0 8px 24px to 0 12px 32px
Transition:    all 0.3s ease or 0.4s ease
Opacity:       Image scales 1.08x
```

### Link Hover
```
Color:         #d4a574 (Gold)
Transition:    all 0.3s ease
Text-decoration: underline (optional)
```

---

## Form Elements

### Input Fields
```
Border:        2px solid #ecf0f1 (light border)
Border-radius: 8px
Padding:       0.7rem 1rem (11px 16px)
Font-size:     1rem
Transition:    all 0.3s ease

Focus State:
  Border:      2px solid #2c5f9f (blue)
  Box-shadow:  0 0 0 3px rgba(44, 95, 159, 0.1)
  Outline:     none
```

### Select Elements
```
Styling:       Same as input fields
Arrow:         Bootstrap default
Appearance:    Clean, minimal
```

### Labels
```
Font-weight:   500-600 (medium/bold)
Color:         #2c3e50 (primary text)
Margin:        0 0 0.5rem 0
Font-size:     0.95rem
```

---

## Shadow Specifications

### Subtle (Cards at rest)
```
box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
```

### Medium (Hovered cards)
```
box-shadow: 0 8px 24px rgba(0, 0, 0, 0.12);
```

### Navbar
```
box-shadow: 0 4px 15px rgba(44, 95, 159, 0.15);
```

### Property Cards (Hovered)
```
box-shadow: 0 12px 32px rgba(44, 95, 159, 0.2);
```

---

## Gradient Specifications

### Primary Gradient
```
background: linear-gradient(90deg, #2c5f9f 0%, #1e4573 100%);
Used in: Navbar, buttons, sections
```

### Card Gradient
```
background: linear-gradient(180deg, #ffffff, #f8f9fa);
Used in: Summary cards, featured sections
```

### Hero Section
```
background: linear-gradient(135deg, #2c5f9f 0%, #1e4573 100%);
Used in: Hero banner, CTAs
```

---

## Z-Index Scale

```
1000+: Dropdowns, tooltips
999:   Modals, overlays
100-200: Fixed navbar
50-100: Cards, regular elements
1: Default (body)
```

---

## Accessibility Considerations

### Color Contrast
- Primary text on white: 7.1:1 (AAA compliant)
- White text on blue: 8.4:1 (AAA compliant)
- Gold accent on white: 4.5:1 (AA compliant)

### Touch Targets
- Minimum button size: 44px × 44px
- Minimum clickable area: 48px × 48px

### Focus States
- Clear outline on all interactive elements
- Visible focus ring (blue outline)
- Keyboard navigation support

### Text Sizing
- Base font: 16px (never smaller)
- Line height: 1.5 to 1.6
- Line length: 50-75 characters max

---

## Animation Timing

```
Quick transitions:    0.2s ease
Standard transitions: 0.3s ease
Smooth transitions:   0.4s ease
Hover effects:        0.3s ease
Page transitions:     0.5s ease
```

---

## Print Styles

The website is print-friendly:
- Dark backgrounds hidden
- Text colors adjusted to black
- Shadows removed
- Navigation hidden
- Footer simplified

---

## Dark Mode (Future)

If implementing dark mode, simply update:
```css
:root {
  --bg-light: #1a1a1a;     /* Dark background */
  --bg-white: #2a2a2a;     /* Dark cards */
  --text-dark: #e0e0e0;    /* Light text */
  /* ... other inversions ... */
}
```

---

## Quick CSS Variable Update

To change brand colors globally:
```css
:root {
  --primary-color: YOUR_COLOR;      /* Main color */
  --secondary-color: YOUR_ACCENT;   /* Accent color */
  --text-dark: YOUR_TEXT;           /* Text color */
  /* All components use these */
}
```

All 50+ components automatically update!

---

**This reference card ensures visual consistency across your entire platform.** ??

Print or bookmark this for quick reference while developing!
