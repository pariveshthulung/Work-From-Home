import "bootstrap/dist/css/bootstrap.css";
import { useState, useEffect } from "react";
import { useOktaAuth } from "@okta/okta-react";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import httpClient from "../Api/HttpClient";
import { useLocation } from "react-router-dom";

import { useNavigate } from "react-router-dom";
export default function ChangePassword() {
  const [email, setEmail] = useState("");
  const [newPassword, setNewPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [currentPassword, setCurrentPassword] = useState("");

  const { oktaAuth, authState } = useOktaAuth();

  const navigate = useNavigate();
  const location = useLocation();

  useEffect(() => {
    if (location.state?.isSuccess) {
      toast.success(location.state?.showToast);
    } else toast.error(location.state?.showToast);

    navigate(location.pathname, { replace: true, state: null });
  }, []);

  async function handleSubmit(e) {
    e.preventDefault();
    try {
      const response = await httpClient.put("/api/Auth/updatepassword", {
        currentPassword,
        email,
        newPassword,
        confirmPassword,
      });
      console.log(response);
      const token = response.data;
      console.log(token);
      localStorage.setItem("accessToken", token.accessToken);
      localStorage.setItem("refreshToken", token.refreshToken);
      navigate("/login", {
        state: { showToast: "Password changed successfull!!!" },
      });

      console.log(response);
    } catch (err) {
      console.log(err);
      if (err.response && Array.isArray(err.response.data.errors)) {
        err.response.data.errors.forEach((errorObj) => {
          toast.error(`${errorObj.fieldName}: ${errorObj.descriptions}`);
        });
      } else {
        toast.error("something went wrong!!!");
      }
    }
  }

  return (
    <section className="vh-100 gradient-custom">
      <div className="container py-5 h-100">
        <ToastContainer />
        <div className="row d-flex justify-content-center align-items-center h-100">
          <div className="col-12 col-md-8 col-lg-6 col-xl-5">
            <div
              className="card bg-light text-dark"
              style={{ borderRadius: "1rem" }}
            >
              <div className="card-body p-5 text-center">
                <div className="px-5 ms-xl-4 mb-5">
                  <span className="h1 fw-bold mb-0">WorkFromHome</span>
                </div>

                <div className="px-5 ms-xl-4 mb-4">
                  <span className="h4 mb-0">Change Password</span>
                </div>

                <form>
                  <div data-mdb-input-init className="form-outline mb-4">
                    <input
                      type="email"
                      className="form-control"
                      value={email}
                      onChange={(e) => setEmail(e.target.value)}
                    />
                    <label className="form-label" htmlFor="form2Example1">
                      Email address
                    </label>
                  </div>
                  <div data-mdb-input-init className="form-outline mb-4">
                    <input
                      type="password"
                      className="form-control"
                      value={currentPassword}
                      onChange={(e) => setCurrentPassword(e.target.value)}
                    />
                    <label className="form-label" htmlFor="form2Example2">
                      Current Password
                    </label>
                  </div>
                  <div data-mdb-input-init className="form-outline mb-4">
                    <input
                      type="password"
                      className="form-control"
                      value={newPassword}
                      onChange={(e) => setNewPassword(e.target.value)}
                    />
                    <label className="form-label" htmlFor="form2Example2">
                      New Password
                    </label>
                  </div>
                  <div data-mdb-input-init className="form-outline mb-4">
                    <input
                      type="password"
                      className="form-control"
                      value={confirmPassword}
                      onChange={(e) => setConfirmPassword(e.target.value)}
                    />
                    <label className="form-label" htmlFor="form2Example2">
                      Confirm Password
                    </label>
                  </div>

                  <button
                    type="button"
                    onClick={(e) => handleSubmit(e)}
                    data-mdb-button-init
                    data-mdb-ripple-init
                    className="btn btn-primary btn-block mb-4"
                  >
                    Change password
                  </button>
                </form>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
}
