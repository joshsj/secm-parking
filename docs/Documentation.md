# Implementation

The solution is built using ASP.NET Core 2.1, with a web front-end and a model-view-controller architecture for the back-end.

# Terminology

*Registration point* <br/>
A machine with which a driver can secure their parked vehicle, i.e., a device to "enter the bay number along with a password"

# Questions

1. How are driver passwords secured?
    - Hashed?
    - Salted?
2) Do driver passwords have requirements?
    - Minimum length?
    - Case variance?
3. If so, is the password input hardware limited?
    - Only alphanumeric?
    - Alphanumeric & specials
4. How do drivers regain access to their car if a password is lost/forgotten?

# Assumptions

1. Driver passwords are cryptographically hashed with a salt.

2) Driver passwords are required to have a minimum of 8 characters. <br/>
In this use-case, a malicious attack physical access to a drivers' car, and potentially a driver's car keys. Considering the additional logistics of an attack, password requirements can be more lenient than software-only measures. 

3. The simulation will not have limited character input. 

4. The system optionally stores the drivers' phone number, entered when securing their car at a registration point. Password loss is resolved as follows:

    -  If a driver registered a phone number, a one-time passcode (OTP) will be sent to their phone. This is simulated with a hard-coded OTP passcode. 

    - If a driver did not register a phone number, the user will be instructed to seek further help.
