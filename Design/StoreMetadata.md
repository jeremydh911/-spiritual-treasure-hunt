# Store Listing Metadata

This document collects fields needed for Play Store / App Store submissions as part of release preparation.

## Common information
- App name
- Short description / tagline
- Full description
- Category (Education, Family, etc.)
- App icon (512x512 PNG for Play Store, 1024x1024 JPG/PNG for App Store)
- Screenshots (phone, tablet, landscape, portrait)
- Feature graphic / promotional banner (Play Store)
- Privacy policy URL
- Contact email / website
- Version number and build code
- Minimum OS versions (Android API level, iOS deployment target)

## Play Store specific
- Content rating questionnaire answers
- AdMob/ads config (if applicable)
- Android package name (e.g., com.example.spiritualtreasure)
- Signing key / keystore path and passwords
- In-app products / subscriptions (define IDs)
- Privacy policy localized text

## App Store specific
- Bundle identifier (e.g., com.example.spiritualtreasure)
- App Store Connect account information
- Review notes / demo account credentials
- Age rating classification form
- Localized app descriptions
- TestFlight beta instructions

## Next actions
1. Fill template with actual copy for the first release.
2. Add graphics into `Design/` or `assets/` directory.
3. Ensure export presets in Godot (`project.godot`) match package/bundle IDs.
4. Update build scripts above with correct godot path, presets, and signing configurations.
