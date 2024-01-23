import React from 'react'
import { UsersTable } from 'widgets/users-table';

import styles from './UsersPage.module.scss'
import { Filter } from 'widgets/filter';
import { useAxios } from 'shared/hooks';
import { userModel } from 'entities/user';

const UsersPage = () => {
  const {response, loading, error} = useAxios<userModel.User[]>('https://backend-seaz96.kexogg.ru/api/accounts/list')

  if(loading) return <></>

  if(response)
    return (
      <div className='container'>
          <section className={styles.filterSection}>
              <Filter />
          </section>
          <section className={styles.gostSection}>
            <UsersTable users={response}/>
          </section>
      </div>
    ) 
  else return <></>
}

export default UsersPage;