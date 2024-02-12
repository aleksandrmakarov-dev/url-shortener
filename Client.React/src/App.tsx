import "./App.css";
import QueryProvider from "./context/query-provider/QueryProvider";
import RouterProvider from "./context/router-provider/RouterProvider";

function App() {
  return (
    <QueryProvider>
      <RouterProvider />
    </QueryProvider>
  );
}

export default App;
