import "./App.css";
import QueryProvider from "./context/query-provider/QueryProvider";
import RouterProvider from "./context/router-provider/RouterProvider";
import SessionProvider from "./context/session-provider/SessionProvider";
import { Toaster } from "./shared/ui/sonner";

function App() {
  return (
    <QueryProvider>
      <SessionProvider>
        <RouterProvider />
        <Toaster />
      </SessionProvider>
    </QueryProvider>
  );
}

export default App;
