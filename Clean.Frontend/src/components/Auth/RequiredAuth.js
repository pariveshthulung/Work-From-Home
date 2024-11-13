import { useLocation, Navigate } from "react-router-dom";
import { useContext, useEffect, useState } from "react";
import AuthContext from "../../context/AuthProvider";
import httpClient from "../Api/HttpClient";
import { useNavigate } from "react-router-dom";
import "bootstrap/dist/js/bootstrap.bundle.min";
import NavbarComponent from "../common/NavbarComponent";

const RequiredAuth = ({ children }) => {
  const authContext = useContext(AuthContext);
  const [token, setToken] = useState(null);
  const [loading, setLoading] = useState(true);
  const [userLoading, setUserLoading] = useState(true);
  const [active, setActive] = useState("Employee");
  const navigate = useNavigate();
  const location = useLocation();

  useEffect(() => {
    function getToken() {
      let accessToken = window.localStorage.getItem("accessToken");
      if (!accessToken) {
        const oktaTokens = JSON.parse(
          window.localStorage.getItem("okta-token-storage")
        );
        if (oktaTokens && oktaTokens.accessToken) {
          accessToken = oktaTokens.accessToken.accessToken;
        }
      }
      return accessToken;
    }

    const accessToken = getToken();
    if (accessToken) {
      setToken(accessToken);
      localStorage.setItem("accessToken", accessToken);
      console.log("Token obtained: ", accessToken);
    }
    setLoading(false);
  }, []);

  useEffect(() => {
    async function fetchUserProfile() {
      if (!token) return;

      try {
        console.log("Fetching user profile with token: ", token);
        const response = await httpClient.get("/api/Employee/GetLoggedInUser", {
          headers: { Authorization: `Bearer ${token}` },
        });
        const user = response.data;
        console.log("User profile fetched:", user);
        authContext.setAuth(user);
        if (user != null) setUserLoading(true);
      } catch (err) {
        console.error("Error fetching user profile:", err);
        authContext.setAuth(null);
        if (err.response?.status === 401) {
          window.localStorage.clear();
          navigate("/login");
        }
      }
    }

    if (token) {
      fetchUserProfile();
    }
  }, [token]);
  useEffect(() => {
    const dropdownElement = document.getElementById("dropdownMenuLink");

    if (dropdownElement) {
      const dropdown = new window.bootstrap.Dropdown(dropdownElement);
      dropdown.toggle();
    }
  }, []);

  useEffect(() => {
    if (!loading && authContext.auth && !userLoading) {
      const userRole = authContext.auth.userRole;
      if (
        userRole === "Manager" ||
        userRole === "Admin" ||
        userRole === "SuperAdmin" ||
        userRole === "Ceo"
      ) {
      } else {
        navigate("/userRequest", {
          state: { showToast: "Login successfull!!!!" },
        }); // Redirect for regular users
      }
    }
  }, [loading, authContext.auth]);

  const closeDropdown = () => {
    const dropdownToggle = document.getElementById("dropdownMenuLink");
    const dropdown = new window.bootstrap.Dropdown(dropdownToggle);
    dropdown.hide();
  };

  if (loading) {
    return <div>Loading...</div>;
  }

  return token ? (
    <div>
      {/* <nav className="navbar navbar-expand-lg navbar-light bg-light mb-5">
        <div className="container-fluid">
          <a className="navbar-brand" href="#">
            WorkFromHome
          </a>
          <button
            className="navbar-toggler"
            type="button"
            data-bs-toggle="collapse"
            data-bs-target="#navbarSupportedContent"
            aria-controls="navbarSupportedContent"
            aria-expanded="false"
            aria-label="Toggle navigation"
          >
            <span className="navbar-toggler-icon"></span>
          </button>
          <div className="collapse navbar-collapse" id="navbarSupportedContent">
            <ul className="navbar-nav me-auto mb-2 mb-lg-0">

              {authContext.auth &&
              (authContext.auth.userRole === "Admin" ||
                authContext.auth.userRole === "Ceo" ||
                authContext.auth.userRole === "Manager" ||
                authContext.auth.userRole === "SuperAdmin") ? (
                <>
                  <li className="nav-item" style={{ cursor: "pointer" }}>
                    <a
                      className={
                        active === "Employee" ? "nav-link active" : "nav-link"
                      }
                      onClick={() => {
                        setActive("Employee");
                        navigate("/");
                      }}
                      aria-current="page"
                    >
                      Employee
                    </a>
                  </li>
                  <div className="dropdown my-auto">
                    <a
                      href="#"
                      role="button"
                      id="dropdownMenuLink"
                      data-bs-toggle="dropdown"
                      aria-expanded="false"
                      className={
                        active === "Request"
                          ? "nav-link active dropdown my-auto dropdown-toggle"
                          : "nav-link dropdown my-auto dropdown-toggle"
                      }
                    >
                      Request
                    </a>
                    <ul
                      className="dropdown-menu"
                      aria-labelledby="dropdownMenuLink"
                    >
                      
                      <li>
                        <a
                          style={{ cursor: "pointer" }}
                          className="dropdown-item"
                          onClick={() => {
                            setActive("Request");
                            navigate("/approveRequest");
                            closeDropdown();
                          }}
                          data-bs-dismiss="dropdown-menu"
                        >
                          Approve Request
                        </a>
                      </li>
                      <li>
                        <a
                          style={{ cursor: "pointer" }}
                          className="dropdown-item"
                          onClick={() => {
                            setActive("Request");
                            navigate("/userRequest");
                            closeDropdown();
                          }}
                          data-bs-dismiss="dropdown-menu"
                        >
                          My Request
                        </a>
                      </li>
                      <li>
                        <a
                          style={{ cursor: "pointer" }}
                          className="dropdown-item"
                          onClick={() => {
                            setActive("Request");
                            navigate("/listRequest");
                            closeDropdown();
                          }}
                        >
                          All Request
                        </a>
                      </li>
                    </ul>
                  </div>
                </>
              ) : (
                <a
                  style={{ cursor: "pointer" }}
                  className={
                    active === "Request" ? "nav-link active" : "nav-link"
                  }
                  onClick={() => {
                    setActive("Request");
                    navigate("/userRequest");
                  }}
                  aria-current="page"
                >
                  Request
                </a>
              )}
            </ul>
            <div className="mx-3">
              <div className="dropdown dropstart">
                <svg
                  href="#"
                  role="button"
                  id="dropdownMenuLink"
                  data-bs-toggle="dropdown"
                  aria-expanded="false"
                  xmlns="http://www.w3.org/2000/svg"
                  width="20"
                  height="20"
                  fill="currentColor"
                  className="bi bi-person-lines-fill"
                  viewBox="0 0 16 16"
                >
                  <path d="M6 8a3 3 0 1 0 0-6 3 3 0 0 0 0 6m-5 6s-1 0-1-1 1-4 6-4 6 3 6 4-1 1-1 1zM11 3.5a.5.5 0 0 1 .5-.5h4a.5.5 0 0 1 0 1h-4a.5.5 0 0 1-.5-.5m.5 2.5a.5.5 0 0 0 0 1h4a.5.5 0 0 0 0-1zm2 3a.5.5 0 0 0 0 1h2a.5.5 0 0 0 0-1zm0 3a.5.5 0 0 0 0 1h2a.5.5 0 0 0 0-1z" />
                </svg>
                <ul
                  style={{ cursor: "pointer" }}
                  className="dropdown-menu"
                  aria-labelledby="dropdownMenuLink"
                >
                  <li>
                    <a
                      className="dropdown-item"
                      onClick={() => {
                        setActive("");
                        navigate("/userProfile");
                      }}
                    >
                      Profile
                    </a>
                  </li>
                  <li>
                    <a
                      className="dropdown-item"
                      onClick={() => {
                        window.localStorage.clear();
                        navigate("/login");
                      }}
                    >
                      Logout
                    </a>
                  </li>
                </ul>
              </div>
            </div>
          </div>
        </div>
      </nav> */}
      <NavbarComponent />
      {children}
    </div>
  ) : (
    <Navigate to="/login" state={{ from: location }} replace />
  );
};

export default RequiredAuth;
