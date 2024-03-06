using PRN221_ASSIGNMENT.Models;

namespace PRN221_ASSIGNMENT.Management
{
    public class TeacherManagement
    {

        private static TeacherManagement instance = null;
        private static readonly object instanceLock = new object();

        private TeacherManagement() { }
        public static TeacherManagement Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new TeacherManagement();
                    }
                    return instance;
                }
            }
        }

        public List<Teacher> GetAll()
        {
            List<Teacher> Teachers;
            //var mapper = AutoMapperConfig.Initialize();

            try
            {
                var context = new ScheduleManagementContext();
                Teachers = context.Teachers.ToList();
                //Customers = mapper.Map<List<CustomerListDTO>>(customers);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Teachers;
        }
    }
}
