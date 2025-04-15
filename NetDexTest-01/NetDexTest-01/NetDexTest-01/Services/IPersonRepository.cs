namespace NetDexTest_01.Services
{

    /// <summary>
    /// Interface that defines the person repository.
    /// </summary>
    /// <remarks>
    /// 
    /// <code>IUserRepository.cs</code> has child files:
    /// 
    /// <list type="bullet">
    /// <listheader>
    /// <!--Contains children(partial) files:-->
    /// </listheader>
    /// <!--                -->
    /// <item>
    /// <term> IPersonRepository.ReadOne.cs </term>
    /// <description> ReadOne for DexHolder </description>
    /// </item>
    /// /// <!---->
    /// <item>
    /// <term> IPersonRepository.Create.cs </term>
    /// <description> Create - Primary working Create and Batch Creation methods. </description>
    /// </item></list>
    /// <!---->
    /// </remarks>
    public partial interface IPersonRepository
    {
        // create just a person // --> Create

        // create a person with ContactInfo, FullName, RecordCollector

            // create a P & Ci,Fn,Rc using Authorixation

        // read just a person // --> ReadOne

        // read all persons

        // read all persons by username

        // read all persons by userid

        
        // update/edit


        // delete a person
    }
}
