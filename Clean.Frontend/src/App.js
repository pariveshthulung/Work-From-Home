import { useEffect, useState } from "react";
import "bootstrap/dist/css/bootstrap.css";
import Login from "./components/Auth/Login";
import Register from "./components/Auth/Register";
import EmployeeDetails from "./components/Employee/EmployeeDetails";
import EmployeeIndex from "./components/Employee/EmployeeIndex";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import AddEmployee from "./components/Employee/AddEmployee";
import SubmitRequest from "./components/Request/SubmitRequest";
import ListRequest from "./components/Request/listRequest";
import UserRequest from "./components/Request/userRequest";
import RequiredAuth from "./components/Auth/RequiredAuth";
import UserDashboard from "./components/Dashboard/UserDashboard";
import PendingRequest from "./components/Request/PendingRequest";
import Button from "./components/common/Button";
import EditEmployee from "./components/Employee/EditEmployee";
import UserProfile from "./components/Employee/UserProfile";
import { Security, LoginCallback } from "@okta/okta-react";
import oktaConfig from "./config";
import { OktaAuth } from "@okta/okta-auth-js";
import ChangePassword from "./components/Auth/ChangePassword";
import ApproveRequest from "./components/Request/ApproveRequest";
import AuthCallback from "./components/Auth/AuthCallback";

function App() {
  const [employee, setEmployee] = useState([]);
  const oktaAuth = new OktaAuth(oktaConfig);

  const restoreOriginalUri = async (_oktaAuth, originalUri) => {
    window.location.replace(originalUri || "/", { replace: true });
  };

  return (
    <div className="mx-5 mt-5">
      <Router>
        <Security oktaAuth={oktaAuth} restoreOriginalUri={restoreOriginalUri}>
          <Routes>
            <Route path="/callback" element={<LoginCallback />} />
            <Route
              path="/"
              element={
                <RequiredAuth>
                  <EmployeeIndex
                    button={
                      <Button
                        name={"Register new Employee"}
                        goto={"/addEmployee"}
                      />
                    }
                  />
                </RequiredAuth>
              }
            />
            <Route
              path="/addEmployee"
              element={
                <RequiredAuth>
                  <AddEmployee />
                </RequiredAuth>
              }
            />
            <Route
              path="/pendingRequest"
              element={
                <RequiredAuth>
                  <PendingRequest />
                </RequiredAuth>
              }
            />
            <Route path="/changePassword" element={<ChangePassword />} />
            <Route
              path="/employeeDetails/:id?"
              element={
                <RequiredAuth>
                  <EmployeeDetails />
                </RequiredAuth>
              }
            />
            <Route
              path="/userProfile/:id?"
              element={
                <RequiredAuth>
                  <UserProfile />
                </RequiredAuth>
              }
            />
            <Route path="/login" element={<Login />} />
            <Route
              path="/register"
              element={
                <RequiredAuth>
                  <Register />
                </RequiredAuth>
              }
            />
            <Route
              path="/editEmployee/:id"
              element={
                <RequiredAuth>
                  <EditEmployee />
                </RequiredAuth>
              }
            />
            <Route
              path="/submitRequest"
              element={
                <RequiredAuth>
                  <SubmitRequest />
                </RequiredAuth>
              }
            />
            <Route
              path="/listRequest"
              element={
                <RequiredAuth>
                  <ListRequest />
                </RequiredAuth>
              }
            />
            <Route
              path="/userRequest"
              element={
                <RequiredAuth>
                  <UserRequest />
                </RequiredAuth>
              }
            />
            <Route
              path="/approveRequest"
              element={
                <RequiredAuth>
                  <ApproveRequest />
                </RequiredAuth>
              }
            />
            <Route
              path="/userDashboard"
              element={
                <RequiredAuth>
                  <UserDashboard />
                </RequiredAuth>
              }
            />
            <Route path="/callback" element={<LoginCallback />} />
            {/* <Route path="/callback" component={LoginCallback} /> */}
          </Routes>
        </Security>
      </Router>
      {/* <Login /> */}
      {/* <EmployeeDetails />
      <EmployeeIndex /> */}
      {/* <Register /> */}
      {/* <table className="table table-striped">
        <thead>
          <tr>
            <th scope="col">S.N.</th>
            <th scope="col">Id</th>
            <th scope="col">Email</th>
            <th scope="col">Name</th>
            <th scope="col">Role</th>
          </tr>
        </thead>
        <tbody>
          {employee.map((employee, i) => (
            <tr
              key={employee.id}
              onClick={() => handleRowClick(employee.id)}
              style={{ cursor: "pointer" }}
            >
              <td>{i + 1}</td>
              <td>{employee.id}</td>
              <td>{employee.email}</td>
              <td>{employee.name}</td>
              <td>{employee.userRole}</td>
            </tr>
          ))}
        </tbody>
      </table> */}
    </div>
  );
}

export default App;
