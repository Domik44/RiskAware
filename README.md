# RiskAware

RiskAware was designed and implemented as part of the **Project Management (MPR)** course at **Brno University of Technology, Faculty of Information Technology (BUT FIT)**.

RiskAware is web application designed to assist IT project teams in identifying, analyzing and managing risks effectively. It provides tools for risk assessment, categorization and visualization, enabling teams to mitigate potential threats and improve project outcomes.

---

## Features
- **Risk Identification:** Users can log and describe potential risks within their projects
- **Risk Analysis:** Supports qualitative risk assessment based on probability and impact
- **Risk Registry:** A centralized database to manage identified risks, assign responsibility and track changes
- **Visual Risk Mapping:** Interactive risk matrices for better prioritization
- **User Authentication & Roles:** Secure login with role-based permissions (admin, project manager, risk manager, team member, external user)
- **Project & Risk Monitoring:** Track the most critical risks over time with filtering and reporting tools

---

## Technology Stack
- **Frontend:** React.js
- **Backend:** ASP.NET Core API
- **Database:** SQL Server

---

## Installation & Setup
1. **Clone the repository**
   ```sh
   git clone https://github.com/yourusername/RiskAware.git
   cd RiskAware
   ```
2. **Backend Setup**
   - Install .NET 7 SDK
   - Configure MySQL database connection in `appsettings.json`
   - Run migrations:
     ```sh
     dotnet ef database update
     ```
   - Start the API:
     ```sh
     dotnet run
     ```
3. **Frontend Setup**
   - Install dependencies:
     ```sh
     npm install
     ```
   - Start the frontend server:
     ```sh
     npm start
     ```

---

## Usage
1. Navigate to `http://localhost:3000/`
2. Log in using your credentials
3. Create a new project and define associated risks
4. Utilize risk categorization, visual analysis and monitoring tools

---

## User Roles & Permissions
| Role | Permissions |
|------|------------|
| **Administrator** | Manage user accounts, assign project managers |
| **Project Manager** | Manage projects, assign team members, oversee risks |
| **Risk Manager** | Assess risks, update risk registry |
| **Team Member** | Add and manage risks within assigned project phases |
| **External User** | View project and risk details, add comments |

---

## Contributors
- **David Drtil** (Team Leader & Frontend)
- **Dominik Pop** (Backend & Frontend)
- **Adam Hos** (Risk Matrix View)
- **Matěj Mudra** (Testing)

---

## Evaluation
Final score: **30/30**
