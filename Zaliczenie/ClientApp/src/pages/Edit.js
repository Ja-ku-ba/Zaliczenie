import React, { useEffect, useState, useContext } from 'react'
import { useNavigate } from 'react-router-dom'
import DatabaseContext from '../contex/DatabaseContext'
import ModelContext from '../contex/ModelContext'

const Edit = () => {
    const nav = useNavigate()
    const { model } = useContext(ModelContext)
    const { database } = useContext(DatabaseContext)
   
    var path = window.location.pathname.split("/");
    const [ art, setArt ] = useState({'relased':'1999-12-21'})      // this value exists because when page is loading, input="date" gets undefined, and throws error
    useEffect(() => {
        fetch(`/${model}/${database}/${path[3]}`)
        .then(res => res.json())
        .then(d => setArt(d))
        .catch(error => console.error("Something went wrong", error));
    }, []);

    async function upload(e) {
      e.preventDefault()
        try {
          const response = await fetch(`${model}/${database}/${art.id}`, {
            method: "PUT",
            body: JSON.stringify(art),
            headers: {
              "Content-Type": "application/json"
            }
          })
          if (!response.ok) {
            alert("Nie dodano elementu, sprawdz połączenie z bazą danych")
          }
          else {
            nav("/")
          }
          
        } catch(error) {
          console.error("Coś poszło nie tak: " + error)
        }
    }

    const handleChange = e => {
      const { name, value } = e.target;
      if (( name === 'author') && value.length > 254) {
        alert("Przekroczono maksymalną liczbę znaków dla autora (254)");
        return;
      }
      
      if (( name === 'title') && value.length > 510) {
        alert("Przekroczono maksymalną liczbę znaków dla tytułu (510)");
        return;
      } 
      
      if (name === 'rating' & (1 > value | value>10)) {
        alert("Ocena musi zawierać się w przedziale 1-10")
        return
      }
      setArt(prevState => ({
          ...prevState,
          [name]: value
      }));
    }

    return (
    <div>
        <form method='PUT' onSubmit={(e) => upload(e)}>
            <label htmlFor="name">Tytuł: </label>
            <input value={ art.title } onChange={e => handleChange(e) } maxLength="510" name='title' type='text' id='name'/>

            <label htmlFor="artist">Twórca: </label>
            <input value={ art.author } onChange={e => handleChange(e) } maxLength="254" type='text' id='artist' name='author'/>

            <label htmlFor="relaseDate">Data wydania: </label>
            <input value={ art.relased.slice(0,10) } onChange={e => handleChange(e) } name='relased' type='date' id='relaseDate'/>

            <label htmlFor='rating'>Ocena: </label>
            <input value={ art.rating } onChange={e => handleChange(e) } name='rating' type='number' min={"1"} max={"10"}/>

            <input type='submit' value={"Potwiedź zmianę"}/>
        </form>
    </div>
  )
}

export default Edit