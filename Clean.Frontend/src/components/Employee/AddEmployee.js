import { useState } from "react";
import httpClient from "../Api/HttpClient";
import "bootstrap/dist/css/bootstrap.css";
import { useEffect } from "react";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { useNavigate } from "react-router-dom";

export default function AddEmployee() {
  const navigate = useNavigate();
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [phoneNo, setPhoneNo] = useState(0);
  const [selectedRole, setSelectedRole] = useState(null);
  const [password, setPassword] = useState("");
  const [street, setStreet] = useState("");
  const [city, setCity] = useState("");
  const [postalCode, setPostalCode] = useState("");
  const [roles, setRoles] = useState([]);

  const notify = (err) => {
    console.log(...err.data);
    const message = err?.data?.description || "Something went wrong!";
    console.log(message);
    toast.error(...err.data);
  };

  useEffect(() => {
    async function fetchStatus() {
      try {
        var response = await httpClient.get(
          "https://localhost:7058/api/Enum/getRoles"
        );
        setRoles(response.data);
      } catch (err) {
        toast.error("Roles: could not fetch roles.");
      }
    }
    fetchStatus();
  }, []);

  const handleAddEmployee = async (e) => {
    try {
      e.preventDefault();
      const response = await httpClient.post("/api/Auth/registerUser", {
        registerEmployeeDto: {
          name: name,
          email: email,
          password: password,
          userRoleId: selectedRole,
          phoneNumber: phoneNo,
          address: {
            street: street,
            city: city,
            postalCode: postalCode,
          },
        },
      });
      console.log(response.data);
      navigate("/", {
        state: { showToast: "Employee added successfully!!!" },
      });
    } catch (err) {
      console.log(err);
      if (err.response && Array.isArray(err.response.data.errors)) {
        err.response.data.errors.forEach((errorObj) => {
          toast.error(`${errorObj.field}: ${errorObj.message}`);
        });
      } else {
        toast.error("something went wrong!!!");
      }
    }
  };

  return (
    <div className="container">
      <ToastContainer />
      <div className="row justify-content-center">
        <div className="col-12 text-center">
          <h1>Register Employee</h1>
        </div>
        <div className="col-md-6 mt-3">
          <form>
            <div className="form-row">
              <div className="row d-flex justify-content-center">
                <div className="form-group col-md-4">
                  <label htmlFor="inputEmail4">Name</label>
                  <input
                    type="text"
                    value={name}
                    onChange={(e) => setName(e.target.value)}
                    className="form-control"
                    required
                  />
                </div>
                <div className="form-group col-md-4">
                  <label htmlFor="inputEmail4">Email</label>
                  <input
                    type="email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    className="form-control"
                    required
                  />
                </div>
              </div>
              <div className="row d-flex justify-content-center">
                <div className="form-group col-md-4">
                  <label htmlFor="dropdown">Select a Role:</label>
                  <select
                    id="dropdown"
                    value={selectedRole}
                    onChange={(e) => setSelectedRole(e.target.value)}
                  >
                    <option value="">Select an option</option>
                    {roles.map((role) => (
                      <option key={role.id} value={role.id}>
                        {role.name}
                      </option>
                    ))}
                  </select>
                </div>
                <div className="form-group col-md-4">
                  <label htmlFor="inputEmail4">Phone number</label>
                  <input
                    type="text"
                    value={phoneNo}
                    required
                    onChange={(e) => setPhoneNo(e.target.value)}
                    className="form-control"
                  />
                </div>
              </div>
            </div>

            <div className="row d-flex justify-content-center">
              <div className="col-md-3">
                <label htmlFor="inputAddress">Street</label>
                <input
                  type="text"
                  value={street}
                  onChange={(e) => setStreet(e.target.value)}
                  className="form-control"
                  required
                />
              </div>
              <div className="col-md-3">
                <label htmlFor="inputCity">City</label>
                <input
                  type="text"
                  value={city}
                  onChange={(e) => setCity(e.target.value)}
                  className="form-control"
                  required
                />
              </div>
              <div className="col-md-2">
                <label htmlFor="inputZip">PostalCode</label>
                <input
                  type="text"
                  value={postalCode}
                  onChange={(e) => setPostalCode(e.target.value)}
                  className="form-control"
                  required
                />
              </div>
            </div>
            <div className="row d-flex justify-content-center">
              <div className="form-group col-md-4 ">
                <label htmlFor="inputPassword4">Password</label>
                <input
                  type="password"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  className="form-control"
                  required
                />
              </div>
              <div className=" form-group col-md-2"></div>
              <div className="form-group col-md-2"></div>
            </div>
            <button
              type="submit"
              style={{ margin: "auto" }}
              onClick={handleAddEmployee}
              className="btn btn-primary my-3 d-flex justify-content-center ml-5"
            >
              Add Employee
            </button>
          </form>
        </div>
      </div>
    </div>
  );
}
