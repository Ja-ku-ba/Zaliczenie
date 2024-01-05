import React, { Component } from 'react';
import { Link, Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import './custom.css';
import { ModelProvider } from './contex/ModelContext';

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <ModelProvider>
        <Link to={"/"}>Home</Link>
        <Routes>
          {AppRoutes.map((route, index) => {
            const { element, ...rest } = route;
            return <Route key={index} {...rest} element={element} />;
          })}
        </Routes>
      </ModelProvider>

    );
  }
}
