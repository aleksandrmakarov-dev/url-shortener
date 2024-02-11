import "./App.css";
import QueryProvider from "./context/query-provider/QueryProvider";
import RouterProvider from "./context/router-provider/RouterProvider";
import SessionProvider from "./context/session-provider/SessionProvider";

function App() {
  return (
    <QueryProvider>
      <SessionProvider>
        <RouterProvider />
      </SessionProvider>
    </QueryProvider>
  );
}

export default App;
