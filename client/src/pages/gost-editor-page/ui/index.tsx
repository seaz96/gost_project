import React from 'react'
import { Filter } from 'widgets/filter';
import GostForm from 'widgets/gost-form/ui';

import styles from './GostEditorPage.module.scss'
import axios from 'axios';
import { newGostModel } from 'widgets/gost-form';
import classNames from 'classnames';
import { useNavigate } from 'react-router-dom';
import { axiosInstance } from 'shared/configs/axiosConfig';

const GostEditorPage = () => {
  const navigate = useNavigate()

  const addNewDocument = (gost: newGostModel.GostToSave) => {
    axiosInstance.post('/docs/add', gost)
    .then(responce => navigate('/gost-review/'+responce.data))
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