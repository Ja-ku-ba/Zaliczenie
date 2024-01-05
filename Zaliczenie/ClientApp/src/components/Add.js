import React, { useState, useContext, useEffect } from 'react';
import ModelContext from '../contex/ModelContext';

const Add = () => {
  const { model, setModel, setUploaded } = useContext(ModelContext);
  const  [formSubmitted, setFormSubmitted] = useState(false)

  const [art, setArt] = useState({
    'id':'',
    'title': '',
    'author': '',
    'rating': "",
    'relased': ""
  });

  const handleChange = e => {
    const { name, value } = e.target;
    if (name === 'rating' && (1 > value || value > 10)) {
      alert('Ocena musi zawierać się w przedziale 1-10');
      return;
    }
    setArt(prevState => ({
      ...prevState,
      [name]: value
    }));
  };

  const upload = async (e) => {
    e.preventDefault()
    try {
      console.log(art)
      const response = await fetch(`${model}`, {
        method: 'POST',
        body: JSON.stringify(art),
        headers: {
          'Content-Type': 'application/json'
        }
      });
      console.log(2, response)

      if (!response.ok) {
        console.log(3)
        alert('Nie dodano elementu, sprawdz połączenie z bazą danych');
      } else {
        console.log(4)
        setUploaded(true)
        setFormSubmitted(true)
        // window.location.reload(false);
      }
      console.log(5)
    } catch(error) {
      console.log('Coś poszło nie tak: ' + error.message);
    }
  };

  useEffect(() => {
    if (formSubmitted) {
      setArt({
        'id': '',
        'title': '',
        'author': '',
        'rating': "",
        'relased': ""
      });
      setFormSubmitted(false);
    }
  }, [formSubmitted]);

  return (
    <div id='Add'>
      <hr/>
      <span>Dodaj nowy element: </span>
      <form onSubmit={(e) => upload(e)}>
        <label htmlFor='name'>Tytuł: </label>
        <input onChange={(e) => handleChange(e)} name='title' type='text' id='name' value={art.title} required/>

        <label htmlFor='artist'>Twórca: </label>
        <input onChange={(e) => handleChange(e)} type='text' id='artist' name='author' value={art.author} required/>

        <label htmlFor='relaseDate'>Data wydania: </label>
        <input onChange={(e) => handleChange(e)} name='relased' type='date' id='relaseDate' value={art.relased} required/>

        <label htmlFor='rating'>Ocena: </label>
        <input onChange={(e) => handleChange(e)} name='rating' type='number' min={'1'} max={'10'} value={art.rating} required/>

        <input type='submit' placeholder={'Dodaj do bazy danych'}/>
      </form>
      <hr/>
    </div>
  );
};

export default Add;