import Delete from "./pages/Delete";
import Edit from "./pages/Edit";
import Home from "./pages/Home";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/edit/:model/:id',
    element: <Edit/>
  },
  {
    path: '/delete/:model/:id',
    element: <Delete/>
  }
];

export default AppRoutes;
