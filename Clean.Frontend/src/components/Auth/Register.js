import "bootstrap/dist/css/bootstrap.css";
import { useState } from "react";

export default function Register() {
  const [name, setName] = useState();
  const [email, setEmail] = useState();
  const [phoneNo, setPhoneNo] = useState();
  const [role, setRole] = useState();
  const [password, setPassword] = useState();
  const [street, setStreet] = useState();
  const [city, setCity] = useState();
  const [postalCode, setPostalCode] = useState();
  return (
    <div className="container">
      <div className="row justify-content-center ">
        <form className="col-md-3">
          <div className="form-group">
            <label for="exampleInputEmail1">Name</label>
            <input
              type="text"
              className="form-control"
              aria-describedby="emailHelp"
            />
          </div>
          <div className="form-group">
            <label for="exampleInputEmail1">Email address</label>
            <input
              type="email"
              className="form-control"
              id="exampleInputEmail1"
              aria-describedby="emailHelp"
            />
          </div>
          <div className="form-group">
            <label for="exampleInputEmail1">Role</label>
            <input
              type="text"
              className="form-control"
              aria-describedby="emailHelp"
            />
          </div>
          <div className="form-group">
            <label for="exampleInputEmail1">Phone Number</label>
            <input
              type="number"
              className="form-control"
              aria-describedby="emailHelp"
            />
          </div>
          <div className="form-group">
            <label for="exampleInputEmail1">Street</label>
            <input
              type="text"
              className="form-control"
              aria-describedby="emailHelp"
            />
          </div>
          <div className="form-group">
            <label for="exampleInputEmail1">City</label>
            <input
              type="text"
              className="form-control"
              aria-describedby="emailHelp"
            />
          </div>
          <div className="form-group">
            <label for="exampleInputEmail1">PostalCode</label>
            <input
              type="text"
              className="form-control"
              aria-describedby="emailHelp"
            />
          </div>
          <div className="form-group">
            <label for="exampleInputPassword1">Password</label>
            <input type="password" className="form-control" />
          </div>
          <button type="submit" className="btn btn-primary my-2">
            Submit
          </button>
        </form>
      </div>
    </div>
  );
}
