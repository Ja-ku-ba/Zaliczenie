import React, { useContext, useEffect, useState } from 'react';
import ModelContext from '../contex/ModelContext';

const Select = () => {
  const { model, setModel } = useContext(ModelContext);
  const [marked, setMarked] = useState([false, false]);       // [song, model], false means that none is selected

  const ifSelected = (e, idx) => {
    setModel(e.target.id);
    setMarked([idx === 0, idx === 1]);
  };
  useEffect(() => {
    if (model === "song") {
      setMarked([true, false])
    } else if (model === "movie") {
      setMarked([false, true])
    } else {
      setMarked([false, false])
    }
  }, [model])

  return (
    <div>
      <hr />
      {model !== "" ? <span>Zmień model</span> : <span>Wybierz model</span>}

      <form>
        <input
          id='song'
          onChange={(e) => ifSelected(e, 0)}
          name='art'
          type='radio'
          checked={marked[0]}
        />
        <label htmlFor='song'>Muzyka</label>
        <br/>
        <input
          id='movie'
          onChange={(e) => ifSelected(e, 1)}
          name='art'
          type='radio'
          checked={marked[1]}
        />
        <label htmlFor='movie'>Film</label>
      </form>
    </div>
  );
};

export default Select;
