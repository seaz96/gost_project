import React from 'react'
import { Filter } from 'widgets/filter';
import GostForm from 'widgets/gost-form/ui';

import styles from './GostEditorPage.module.scss'
import axios from 'axios';
import { newGostModel } from 'widgets/gost-form';
import classNames from 'classnames';

const GostEditorPage = () => {

  const addNewDocument = (gost: newGostModel.GostToSave) => {
    axios.post('https://backend-seaz96.kexogg.ru/api/docs/add', gost, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem('jwt_token')}`
      }
    })
    .then(responce => console.log(responce))
  }

  return (
    <div className='container'>
      <section className={classNames('contentContainer', styles.reviewSection)}>
        <GostForm handleSubmit={addNewDocument}/>
      </section>
    </div>
  )
}

export default GostEditorPage;