import { useNavigate } from "react-router-dom";
import RequestDropdown from "./RequestDropdown";
import { useState, useContext } from "react";
import useAuth from "../hooks/useAuth";
import AuthContext from "../../context/AuthProvider";
import UserProfileComponent from "./UserProfileComponent";

const NavbarComponent = () => {
  const authContext = useContext(AuthContext);
  const { auth } = useAuth(AuthContext);

  const [active, setActive] = useState(() => {
    if (
      auth?.userRole === "Manager" ||
      auth?.userRole === "Admin" ||
      auth?.userRole === "SuperAdmin" ||
      auth?.userRole === "Ceo"
    ) {
      return "Employee";
    } else {
      return "Request";
    }
  });
  const navigate = useNavigate();

  return (
    <nav className="navbar navbar-expand-lg navbar-light bg-light mb-5">
      <div className="container-fluid">
        <a
          style={{ cursor: "pointer" }}
          className="navbar-brand"
          onClick={() => {
            setActive("Request");
            navigate("/userRequest");
          }}
        >
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
                  >
                    Employee
                  </a>
                </li>
                <RequestDropdown setActive={setActive} active={active} />
                <li className="nav-item" style={{ cursor: "pointer" }}>
                  <a
                    className={
                      active === "SubmitRequest"
                        ? "nav-link active"
                        : "nav-link"
                    }
                    onClick={() => {
                      setActive("SubmitRequest");
                      navigate("/submitRequest");
                    }}
                  >
                    Submit Request
                  </a>
                </li>
              </>
            ) : (
              <>
                {/* {setActive("Request")} */}
                <a
                  style={{ cursor: "pointer" }}
                  className={
                    active === "Request" ? "nav-link active" : "nav-link"
                  }
                  onClick={() => {
                    setActive("Request");
                    navigate("/userRequest");
                  }}
                >
                  Request
                </a>
                <a
                  style={{ cursor: "pointer" }}
                  className={
                    active === "SubmitRequest" ? "nav-link active" : "nav-link"
                  }
                  onClick={() => {
                    setActive("SubmitRequest");
                    navigate("/submitRequest");
                  }}
                >
                  Submit Request
                </a>
              </>
            )}
          </ul>
          <UserProfileComponent />
        </div>
      </div>
    </nav>
  );
};

export default NavbarComponent;
