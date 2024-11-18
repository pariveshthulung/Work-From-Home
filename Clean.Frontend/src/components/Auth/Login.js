import "bootstrap/dist/css/bootstrap.css";
import { useState, useEffect } from "react";
import { useOktaAuth } from "@okta/okta-react";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import httpClient from "../Api/HttpClient";
import { useLocation } from "react-router-dom";
import useAuth from "../hooks/useAuth";
import AuthContext from "../../context/AuthProvider";

import { useNavigate } from "react-router-dom";
export default function Login() {
  const [email, setEmail] = useState("");
  const [loading, setLoading] = useState(true);
  const [password, setPassword] = useState("");
  const { oktaAuth, authState } = useOktaAuth();
  const authContext = useAuth(AuthContext);

  const navigate = useNavigate();
  const location = useLocation();

  useEffect(() => {
    if (location.state?.showToast) {
      toast.success(location.state?.showToast);
    }
    navigate(location.pathname, { replace: true, state: null });
  }, []);

  async function handleOktaLogin() {
    try {
      const transaction = await oktaAuth.signInWithRedirect();

      if (transaction.status === "SUCCESS") {
        const { accessToken, idToken } = transaction.tokens;
        console.log(transaction.tokens);

        localStorage.setItem("accessToken", accessToken.accessToken);
        localStorage.setItem("idToken", idToken.idToken);
      } else {
        throw new Error("Login failed");
      }
    } catch (err) {
      console.error("Okta login error:", err);
    }
  }

  // async function handleSubmit(e) {
  //   e.preventDefault();
  //   try {
  //     const response = await httpClient.post("/api/Auth/loginUser", {
  //       loginDto: {
  //         email: email,
  //         password: password,
  //       },
  //     });
  //     console.log(response);
  //     const token = response.data;
  //     console.log(token);
  //     localStorage.setItem("accessToken", token.accessToken);
  //     localStorage.setItem("refreshToken", token.refreshToken);
  //     if (authContext.auth) {
  //       const userRole = authContext.auth.userRole;
  //       if (
  //         userRole === "Manager" ||
  //         userRole === "Admin" ||
  //         userRole === "SuperAdmin" ||
  //         userRole === "Ceo"
  //       ) {
  //         navigate("/listRequest", {
  //           state: { showToast: "Login successfull!!!!" },
  //         });
  //       } else {
  //         navigate("/submitRequest", {
  //           state: { showToast: "Login successfull!!!!" },
  //         });
  //       }
  //     }
  //     // navigate("/listRequest", { replace: true });
  //     console.log(response);
  //   } catch (err) {
  //     console.log(err);
  //     if (err.response.status === 403) {
  //       navigate("/changePassword", {
  //         state: {
  //           isSuccess: false,
  //           showToast: "Your password is expire.Please Update your password!!!",
  //         },
  //       });
  //     }
  //     if (err.response && Array.isArray(err.response.data.errors)) {
  //       err.response.data.errors.forEach((errorObj) => {
  //         toast.error(`${errorObj.fieldName}: ${errorObj.descriptions}`);
  //       });
  //     } else {
  //       toast.error("something went wrong!!!");
  //     }
  //   } finally {
  //     setLoading(false);
  //   }
  // }

  async function handleSubmit(e) {
    e.preventDefault();
    setLoading(true); // Start loading
    try {
      const response = await httpClient.post("/api/Auth/loginUser", {
        loginDto: {
          email,
          password,
        },
      });
      const token = response.data;
      localStorage.setItem("accessToken", token.accessToken);
      localStorage.setItem("refreshToken", token.refreshToken);

      const userResponse = await httpClient.get(
        "/api/Employee/GetLoggedInUser",
        {
          headers: { Authorization: `Bearer ${token.accessToken}` },
        }
      );
      const user = userResponse.data;
      authContext.setAuth(user);

      const userRole = user?.userRole;
      if (
        userRole === "Manager" ||
        userRole === "Admin" ||
        userRole === "SuperAdmin" ||
        userRole === "Ceo"
      ) {
        navigate("/listRequest", {
          state: { showToast: "Login successful!!!!" },
        });
      } else {
        navigate("/userRequest", {
          state: { showToast: "Login successful!!!!" },
        });
      }
    } catch (err) {
      console.error(err);
      if (err.response?.status === 403) {
        navigate("/changePassword", {
          state: {
            isSuccess: false,
            showToast: "Your password is expired. Please update your password!",
          },
        });
      }
      if (err.response && Array.isArray(err.response.data.errors)) {
        err.response.data.errors.forEach((errorObj) => {
          toast.error(`${errorObj.fieldName}: ${errorObj.descriptions}`);
        });
      } else {
        toast.error("Something went wrong!");
      }
    } finally {
      setLoading(false); // End loading
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
              <div className="card-body p-5 ">
                <div className="px-5 ms-xl-4 mb-5 text-center">
                  <span className="h1 fw-bold mb-0">WorkFromHome</span>
                </div>
                <div className="px-5 ms-xl-4 mb-4 text-center">
                  <span className="h4 mb-0">Login</span>
                </div>
                <form>
                  <div data-mdb-input-init className="form-outline mb-4">
                    <label className="form-label" htmlFor="form2Example1">
                      Email address
                    </label>
                    <input
                      type="email"
                      id="form2Example1"
                      className="form-control"
                      value={email}
                      onChange={(e) => setEmail(e.target.value)}
                    />
                  </div>
                  <div data-mdb-input-init className="form-outline mb-4">
                    <label className="form-label" htmlFor="form2Example2">
                      Password
                    </label>
                    <input
                      type="password"
                      id="form2Example2"
                      className="form-control"
                      value={password}
                      onChange={(e) => setPassword(e.target.value)}
                    />
                  </div>
                  <div className="row mb-4">
                    <div className="col text-center">
                      <a
                        style={{ cursor: "pointer" }}
                        onClick={() => navigate("/changePassword")}
                      >
                        Forgot password?
                      </a>
                    </div>
                  </div>
                  <div class="d-flex  flex-column justify-content-center">
                    <button
                      type="button"
                      onClick={(e) => handleSubmit(e)}
                      data-mdb-button-init
                      data-mdb-ripple-init
                      className="btn btn-primary btn-block mb-2"
                    >
                      Sign in
                    </button>
                    {/* </div> */}
                    {/* <div className="d-flex justify-content-center"> */}
                    <hr className="my-4" />
                    <button
                      data-mdb-button-init
                      data-mdb-ripple-init
                      className="btn btn-block btn-primary"
                      type="button"
                      onClick={handleOktaLogin}
                    >
                      <a
                        title="Okta Inc, Public domain, via Wikimedia Commons"
                        href="https://commons.wikimedia.org/wiki/File:Okta_logo_(2023).svg"
                      >
                        <img
                          width="64"
                          alt="Okta logo (2023)"
                          src="https://upload.wikimedia.org/wikipedia/commons/thumb/8/83/Okta_logo_%282023%29.svg/512px-Okta_logo_%282023%29.svg.png?20230712161629"
                        />
                      </a>
                      <i className="fab fa-google me-2"></i> Sign in with okta
                    </button>
                  </div>
                </form>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
}
