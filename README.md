# TokenAuthorization
Build using dotnet core 2.2.

Usage:

**POST /api/auth/api-key** -> Returns API token

Pass result value to **X-Api-Key** header and call

**GET /api/auth/jwt**

Use result JWT token as usual one in header

**Authorization Bearer JWT_Token** to call

**GET /api/protected**

....

PROFIT!
