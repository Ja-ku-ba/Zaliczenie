import React, { useContext } from 'react'

import Add from "../components/Add"
import Display from '../components/Display';
import ModelContext from '../contex/ModelContext';
import Select from '../components/Select';

const Home = () => {
  const { model } = useContext(ModelContext);
  
  return (
    <div>   
      <Select/>
      <div>
        { model !== "" ? 
        <div>
          <Display/>
          <Add/>
        </div> 
        : 
        <span>/|\ Musisz wybrać model</span>}
      </div>
        
    </div>
  )
}

export default Home