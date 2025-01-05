import { UsersTable } from 'widgets/users-table';
import styles from './UsersPage.module.scss'
import { useAxios } from 'shared/hooks';
import { userModel } from 'entities/user';
import classNames from 'classnames';

const UsersPage = () => {
  const {response, loading, error} = useAxios<userModel.User[]>('/accounts/list')

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