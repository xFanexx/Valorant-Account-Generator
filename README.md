# Valorant Account Generator by xFanexx

## Overview

The Valorant Account Generator is a tool designed to automate the creation of Valorant accounts. It generates random email addresses, usernames, and passwords, then uses these details to fill out and submit the registration forms on the Valorant website. The generated account details are saved in text files for easy reference.

## Features

- **Email Generation:** Creates random email addresses.
- **Username Generation:** Generates unique usernames.
- **Password Generation:** Produces secure, random passwords.
- **Automated Registration:** Automatically completes the registration forms on the Valorant website.
- **Account Data Storage:** Saves account details in text files within the `accounts` directory.
- **Clipboard Copying:** Provides buttons to copy the generated email, username, and password to the clipboard.

## Installation

1. **Prerequisites:**
   - [.NET Core SDK](https://dotnet.microsoft.com/download)
   - [Firefox WebDriver](https://github.com/mozilla/geckodriver/releases)
   - [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json)

2. **Clone the Repository:**
   ```sh
   git clone https://github.com/xFanexx/Valorant-Account-Generator.git


# Usage
Launch the Application:
Open the solution file in your preferred IDE (such as Visual Studio) and start the project.

# Generate an Account:
Click the "Generate" button to create a new random email, username, and password. The application will use these credentials to automatically fill out and submit the Valorant registration form.

# Complete CAPTCHA:
After the password is generated and the registration form is submitted, a message will appear notifying you that you might need to solve a CAPTCHA. Complete the CAPTCHA manually to finalize the registration process.

# Save Account Data:
Once the account creation is complete, the details will be saved in a text file located in the accounts folder.

# Copy Data:
Use the "Copy Email", "Copy Username", and "Copy Password" buttons to copy the respective data to your clipboard for convenience.

# Releases
The source code is available in this repository. You can also download the latest release from the Releases tab.

# Contributing
If you would like to contribute to this project, please fork the repository, make your changes, and submit a pull request. All contributions are welcome!

# License
This project is licensed under the MIT License. See the LICENSE file for details.
