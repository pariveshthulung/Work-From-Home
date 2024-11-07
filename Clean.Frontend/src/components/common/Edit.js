// import { useNavigate } from "react-router-dom";
// import { useEffect, useState } from "react";
// import "bootstrap/dist/css/bootstrap.css";
// import httpClient from "../Api/HttpClient";
// import "jquery/dist/jquery.min.js";
// import "bootstrap/dist/js/bootstrap.min.js";
// import "bootstrap/dist/js/bootstrap.bundle";
// import "bootstrap/dist/js/bootstrap.bundle.min";
// import useAuth from "../hooks/useAuth";
// import AuthContext from "../../context/AuthProvider";
// import { ToastContainer, toast } from "react-toastify";
// import "react-toastify/dist/ReactToastify.css";

// export default function Edit({ requests, setRequests, fetchRequest }) {
//   const [deleteId, setDeleteId] = useState(null);
//   const [status, setStatus] = useState([]);
//   const [error, setError] = useState(null);
//   const [loading, setLoading] = useState(false);
//   const navigate = useNavigate();

//   useEffect(() => {
//     async function fetchStatus() {
//       try {
//         var response = await httpClient.get(
//           "https://localhost:44314/api/Enum/getApprovalStatus"
//         );
//         setStatus(response.data);
//       } catch (err) {
//         setError(err.response.data);
//       }
//     }
//     fetchStatus();
//   }, []);

//   const handleOnDeleteClick = (id) => {
//     setDeleteId(id);
//   };

//   const handleStatusClick = async (id, status) => {
//     console.log(status);
//     console.log(id);
//     try {
//       var response = await httpClient.post(
//         `/api/Request/approverequest?requestGuidId=${id}&approve=${status}`
//       );
//       // .finally(() => setLoading(true));
//       console.log(response);
//       toast.success("Request status changed sucessfully!!");
//       await fetchRequest();
//       // console.log(newRequests);
//     } catch (err) {
//       console.log(err);
//       if (err.response && Array.isArray(err.response.data.errors)) {
//         err.response.data.errors.forEach((errorObj) => {
//           toast.error(`${errorObj.field}: ${errorObj.message}`);
//         });
//       } else {
//         toast.error("something went wrong!!!");
//       }
//     }
//   };

//   const handleDeleteConfimation = async () => {
//     console.log("deleyed clicked");
//     try {
//       var response = await httpClient.delete(
//         `/api/Request/deleteRequest?guidId=${deleteId}`
//       );
//       console.log(response);

//       if (response.status === 204) {
//         setRequests((requests) =>
//           requests.filter((x) => x.guidId !== deleteId)
//         );
//         toast.success("Request deleted successfully.");
//       }
//       setDeleteId(null);
//     } catch (err) {
//       if (err.response && Array.isArray(err.response.data.errors)) {
//         err.response.data.errors.forEach((errorObj) => {
//           toast.error(`${errorObj.field}: ${errorObj.message}`);
//         });
//       } else {
//         toast.error("something went wrong!!!");
//       }
//     }
//   };

//   return (
//     <>
//       <td>
//         {/* ---------------------------------------------- */}
//         <div className="dropdown dropend">
//           <svg
//             href="#"
//             role="button"
//             id="dropdownMenuLink"
//             data-bs-toggle={
//               request.approval.approvalStatus === "Accepted" ||
//               request.approval.approvalStatus === "Rejected"
//                 ? "dropdown disabled"
//                 : "dropdown"
//             }
//             aria-expanded="false"
//             xmlns="http://www.w3.org/2000/svg"
//             width="16"
//             height="16"
//             fill="currentColor"
//             className="bi bi-pencil-square"
//             viewBox="0 0 16 16"
//           >
//             <path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z" />
//             <path
//               fillRule="evenodd"
//               d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5z"
//             />
//           </svg>

//           <ul
//             style={{ cursor: "pointer" }}
//             className="dropdown-menu"
//             aria-labelledby="dropdownMenuLink"
//           >
//             {status.length > 0 ? (
//               status
//                 .filter(
//                   (statusItem) =>
//                     statusItem.status === "Accepted" ||
//                     statusItem.status === "Rejected"
//                 )
//                 .map((statusItem) => {
//                   return (
//                     <li
//                       key={statusItem.id}
//                       onClick={() =>
//                         handleStatusClick(request.guidId, statusItem.status)
//                       }
//                     >
//                       <a className="dropdown-item">{statusItem.status}</a>
//                     </li>
//                   );
//                 })
//             ) : (
//               <li>
//                 <a className="dropdown-item">Loading...</a>
//               </li>
//             )}
//           </ul>
//         </div>

//         {/* ---------------------------------------------- */}
//       </td>
//       <td>
//         <svg
//           style={{ cursor: "pointer" }}
//           onClick={() => handleOnDeleteClick(request.guidId)}
//           data-bs-toggle="modal"
//           data-bs-target="#exampleModal"
//           xmlns="http://www.w3.org/2000/svg"
//           width="16"
//           height="16"
//           fill="currentColor"
//           className="bi bi-trash"
//           viewBox="0 0 16 16"
//         >
//           <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0z" />
//           <path d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4zM2.5 3h11V2h-11z" />
//         </svg>
//       </td>
//     </>
//   );
// }
