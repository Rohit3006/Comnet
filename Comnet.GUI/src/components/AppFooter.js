import React from 'react'
import { CFooter } from '@coreui/react'

const AppFooter = () => {
  return (
    <CFooter className="px-4">
      <div>
        <span className="ms-1">Copyright ©️ 2024 Skill Matrix</span>
      </div>
      <div className="ms-auto">
        {/* <span className="me-1">Powered by</span>
        <a href="https://www.eminence-solutions.com/" target="_blank" rel="noopener noreferrer">
          Eminence Solutions
        </a> */}
      </div>
    </CFooter>
  )
}

export default React.memo(AppFooter)
