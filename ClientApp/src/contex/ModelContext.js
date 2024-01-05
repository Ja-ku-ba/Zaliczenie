import React, { useState, createContext, useEffect } from 'react';

const ModelContext = createContext();
export default ModelContext;

export const ModelProvider = ({ children }) => {
  const [model, setModel] = useState('');
  const [uploaded, setUploaded] = useState(false)
  
  useEffect(() => {
    const cookies = document.cookie;

    //Ten warrunek do poprawy
    if (!cookies.includes("model=movie") || !cookies.includes("model=song")) {
      document.cookie = `model=${model}; expires=Fri, 31 Dec 2100 23:59:59 GMT`;
    }

    if (model === "") {
      if (cookies.includes("model=movie")){
        setModel('movie')
      } else if (cookies.includes("model=song")) {
        setModel('song')
      } else {
        setModel("")
      }
    }
  }, [model]);

  const exportContext = {
    model: model,
    setModel: setModel,
    uploaded:uploaded,
    setUploaded: setUploaded
  };

  return (
    <ModelContext.Provider value={exportContext}>
      {children}
      {model}
    </ModelContext.Provider>
  );
};
