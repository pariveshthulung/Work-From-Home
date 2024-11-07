import { useEffect, useState } from "react";
import httpClient from "../Api/HttpClient";
import Request from "./Request";

export default function PendingRequest() {
  const [loading, setLoading] = useState(false);
  const [requests, setRequests] = useState(false);
  async function fetchRequest() {
    try {
      var response = await httpClient
        .get("/api/Request/getAllRequestByStatus?status=Pending")
        .finally(setLoading(false));
      console.log(response);
      setRequests(response.data);
    } catch (err) {
      console.log(err);
    }
  }
  useEffect(() => {
    fetchRequest();
  }, []);

  return (
    !loading && (
      <Request
        heading={"Pending Request"}
        requests={requests}
        fetchRequest={fetchRequest}
        setRequest={setRequests}
      />
    )
  );
}
