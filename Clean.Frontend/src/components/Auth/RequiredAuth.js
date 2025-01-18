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
  useEffect(() => {
    const accessToken = getToken();
    if (accessToken) {
      setToken(accessToken);
      localStorage.setItem("accessToken", accessToken);
      console.log("Token obtained: ", accessToken);
    }
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
      } finally {
        setLoading(false);
      }
    }

    if (token) {
      fetchUserProfile();
    }
  }, [token]);

  useEffect(() => {
    const accessToken = getToken();
    console.log(accessToken);
    if (accessToken === null) return;
    if (authContext.auth) {
      const userRole = authContext.auth.userRole;
      setLoading(false);
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
    }
  }, [authContext.auth]); // React to changes in authContext.auth

  useEffect(() => {
    const dropdownElement = document.getElementById("dropdownMenuLink");

    if (dropdownElement) {
      const dropdown = new window.bootstrap.Dropdown(dropdownElement);
      dropdown.toggle();
    }
  }, []);

  const closeDropdown = () => {
    const dropdownToggle = document.getElementById("dropdownMenuLink");
    const dropdown = new window.bootstrap.Dropdown(dropdownToggle);
    dropdown.hide();
  };

  if (loading) {
    navigate("/login");
    // return <div>Loading...</div>;
  }

  return token ? (
    <div>
      <NavbarComponent />
      {children}
    </div>
  ) : (
    <Navigate to="/login" state={{ from: location }} replace />
  );
};

export default RequiredAuth;
