import React from 'react'
import { UsersTable } from 'widgets/users-table';

import styles from './UsersPage.module.scss'
import { Filter } from 'widgets/filter';
import { useAxios } from 'shared/hooks';
import { userModel } from 'entities/user';
import classNames from 'classnames';

const UsersPage = () => {
  const {response, loading, error} = useAxios<userModel.User[]>('https://backend-seaz96.kexogg.ru/api/accounts/list')

  if(loading) return <></>

  if(response)
    return (
      <div className='container'>
          <section className={classNames('contentContainer', styles.gostSection)}>
            <h2 className={styles.title}>Список пользователей</h2>
            <UsersTable users={response}/>
          </section>
      </div>
    ) 
  else return <></>
}

export default UsersPage;